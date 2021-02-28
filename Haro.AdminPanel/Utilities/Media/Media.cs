namespace Haro.AdminPanel.Utilities.Media
{
    public class Media
    {
        public string Path { get; set; }
        public string FileName { get; set; }
        public string Extension { get; set; }
        public string FullPath => this.Path + this.FileName + "." + this.Extension;
    }
}