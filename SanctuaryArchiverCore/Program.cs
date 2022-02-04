using SanctuaryArchiverCore;
using System.Net.Http.Json;
using System.Text.Json;

Console.WriteLine("Preparing...");

HttpClient client = new HttpClient();
if (!Directory.Exists("sanctuary-archive/"))
    Directory.CreateDirectory("sanctuary-archive/");

List<Asset> assets = await client.GetFromJsonAsync<List<Asset>>("https://sanctuary.moe/assets/@all/json");
Console.WriteLine("Preparing the download of: " + assets.Count + " in 14 pools.");

var options = new JsonSerializerOptions(JsonSerializerDefaults.Web) { WriteIndented = true };
string jsonOriginal = JsonSerializer.Serialize(assets, options);
string jsonNew = JsonSerializer.Serialize(assets.Select(x => new Archived(x)), options);

File.Create("sanctuary-archive/sanctuary.json").Close();
File.Create("sanctuary-archive/sanctuary-raw.json").Close();

await File.WriteAllTextAsync("sanctuary-archive/sanctuary.json", jsonNew);
await File.WriteAllTextAsync("sanctuary-archive/sanctuary-raw.json", jsonOriginal);

int poolInput = -1;
if(args.Length >= 1)
{
    if (!int.TryParse(args[0], out poolInput))
        poolInput = -1;
}

int c = 1;
int poolCount = poolInput == -1 ? 28 : poolInput;
int total = assets.Count;
List<Task> tasks = new List<Task>();
var pools = assets.Split(poolCount);
int poolCt = 0;
foreach (var pool in pools)
{
    var t = Task.Run(() => _ = DownloadPool(poolCt++, pool.ToList()));
    tasks.Add(t);
}

Console.WriteLine("Waiting for pools.");
Task.WaitAll(tasks.ToArray());

Console.WriteLine("DOWNLOADING ARCHIVE FINISHED!");
Console.WriteLine("DOWNLOADING ARCHIVE FINISHED!");
Console.WriteLine("DOWNLOADING ARCHIVE FINISHED!");

async Task DownloadPool(int p, List<Asset> pool)
{
    Console.WriteLine("[{0}/{1}] Pool started!", p, poolCount);
    foreach (var asset in pool)
    {
        try
        {
            int curr = c++;
            using var httpclient = new HttpClient();
            Console.WriteLine("[{0}/{1}] ({2} of {3}) Download: {4}", p, poolCount, curr, total, asset.Name);
            using var resp = await httpclient.GetAsync(asset.URLs.Download);
            string header = resp.Content.Headers.ContentDisposition.FileName;

            string folder = $"sanctuary-archive/{UrlSlugger.ToUrlSlug(asset.Name)}";
            if (Directory.Exists(folder))
                folder = folder + "_" + asset.Id;

            Directory.CreateDirectory(folder);
            using (var fs = File.Open(Path.Combine(folder, header), FileMode.Create, FileAccess.ReadWrite))
                await resp.Content.CopyToAsync(fs);

            Console.WriteLine("[{0}/{1}] ({2} of {3}) Thumbnail: {4}", p, poolCount, curr, total, asset.Name);

            await DownloadImage(httpclient, asset.URLs.Thumbnail, folder, "thumbnail");
            if (asset.URLs.Secondary != null)
            {
                Console.WriteLine("[{0}/{1}] ({2} of {3}) Other Image: {4}", p, poolCount, curr, total, asset.Name);
                await DownloadImage(httpclient, asset.URLs.Secondary, folder, "other");
            }

            resp.Dispose();
            httpclient.Dispose();

            string jsonpath = Path.Combine(folder, "item.json");
            string textpath = Path.Combine(folder, "details.txt");

            File.Create(jsonpath).Close();
            File.Create(textpath).Close();
            await File.WriteAllTextAsync(textpath, CreateDetails(asset));
            await File.WriteAllTextAsync(jsonpath, JsonSerializer.Serialize(new Archived(asset), options));

            Console.WriteLine("[{0}/{1}] ({2} of {3}) Completed: {4}", p, poolCount, curr, total, asset.Name);
            GC.Collect();
        }
        catch
        {
            Console.WriteLine("Error downloading: " + asset.Name);
            GC.Collect();
        }
    }
}

async Task DownloadImage(HttpClient client, string url, string path, string type)
{
    using var resp = await client.GetAsync(url);
    string header = resp.Content.Headers.ContentDisposition.FileName;
    string name = type + Path.GetExtension(header) ?? ".png";

    using (var fs = File.Open(Path.Combine(path, name), FileMode.Create, FileAccess.ReadWrite))
        await resp.Content.CopyToAsync(fs);

    resp.Dispose();
}

string CreateDetails(Asset asset)
{
    string tags = asset.Tags.Length == 0 ? "None" : string.Join(", ", asset.Tags);
    return @$"Name: {asset.Name}
Author Name: {asset.Uploader}
Description:

{(asset.Description ?? "None")}

File Name: {asset.FileName}
File SHA1 Checksum: {asset.FileHash}

Tags: {tags}
Store URL: {(asset.OriginalAssetLink ?? "None")}
YouTube URL: {(asset.YouTubeURL ?? "None")}
Timestamp: {asset.Timestamp} UTC
ID: {asset.Id}";
}