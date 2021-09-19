using System;
public class Paths
{
    private const string azureSpatialAnchors = "AzureSpatialAnchors";
    private const string networkModel = "Mirror";
    private const string networkServer = "MirrorServer";
    private const string custom = "Custom";
    private const string anchor = "AnchorBasedRoot(Clone)";

    public static string AzureSpatialAnchors
    {
        get { return azureSpatialAnchors; }
    }

    public static string NetworkModel
    {
        get { return networkModel; }
    }

    public static string NetworkServer
    {
        get { return networkServer; }
    }

    public static string Custom
    {
        get { return custom; }
    }

    public static string Anchor
    {
        get { return anchor; }
    }
}
