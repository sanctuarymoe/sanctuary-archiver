public class Asset
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string[] Tags { get; set; }
    public string FileHash { get; set; }
    public string FileName { get; set; }
    public string OriginalAssetLink { get; set; }
    public string YouTubeURL { get; set; }
    public string Uploader { get; set; }
    public DateTime Timestamp { get; set; }
    public AssetURLs URLs { get; set; }
}

public class Archived
{
    public Archived(Asset asset)
    {
        Id = asset.Id;
        Name = asset.Name;
        Description = asset.Description;
        Tags = asset.Tags;
        AuthorName = asset.Uploader;
        Filename = asset.FileName;
        Checksum = asset.FileHash;
        StoreURL = asset.OriginalAssetLink;
        YoutubeURL = asset.YouTubeURL;
        Timestamp = asset.Timestamp;
    }

    public string Id { get; set; }
    public string Name { get; set; }
    public string AuthorName { get; set; }
    public string Filename { get; set; }
    public string Checksum { get; set; }
    public string StoreURL { get; set; }
    public string YoutubeURL { get; set; }
    public DateTime Timestamp { get; set; }
    public string Description { get; set; }
    public string[] Tags { get; set; }
}
