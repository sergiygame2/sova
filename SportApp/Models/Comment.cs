using System;

namespace SportApp.Models
{
    public class Comment : IIdentifiable
    {
        public int Id { get; set;}
        public int GymId { get; set; }
        public int UserId { get; set; }
        public string CommentText { get; set; }
        public int Rate { get; set; }
        public DateTime PublicationDate { get; set; }
    }
}