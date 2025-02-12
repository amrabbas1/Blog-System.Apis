namespace BlogSystem.Core.Specifications
{
    public class BlogPostSpecParams
    {
        private string? searchByTag;
        public string? SearchByTag
        {
            get { return searchByTag; }
            set { searchByTag = value?.ToLower(); }
        }

        private string? searchByTitle;
        public string? SearchByTitle
        {
            get { return searchByTitle; }
            set { searchByTitle = value?.ToLower(); }
        }

        private string? searchByCategory;
        public string? SearchByCategory
        {
            get { return searchByCategory; }
            set { searchByCategory = value?.ToLower(); }
        }

        public string? FilterByStatus { get; set; }

    }
}