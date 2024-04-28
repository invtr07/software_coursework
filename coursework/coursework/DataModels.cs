namespace coursework
{
    public class DataModels
    {
        public class AHPItem
        {
            public string Name { get; set; }
            public string Description { get; set; }
            public string[] Children { get; set; }
        }
        public class MenuItems
        {
            public string Title { get; set; }
            public string Descr { get; set; }
        }

        public class Goal
        {
            public string Child { get; set; }
        }

        public class Criterions
        {
            public string Name { get; set; }
            public string Description { get; set; } 
            public decimal LocalImportance { get; set; }
        }

        public class Decisions
        {
            public string Name { get; set; }
            public string Description { get; set; }
            public decimal LocalImportance { get; set; }
            public decimal GlobalImportance { get; set; }
        }

       
        
        
    }
    
}