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

        protected bool Equals(Comment other)
        {
            return Id == other.Id && 
                   GymId == other.GymId && 
                   UserId == other.UserId && 
                   string.Equals(CommentText, other.CommentText) && 
                   Rate == other.Rate && 
                   PublicationDate.Equals(other.PublicationDate);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Comment)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = Id;
                hashCode = (hashCode * 397) ^ GymId;
                hashCode = (hashCode * 397) ^ UserId;
                hashCode = (hashCode * 397) ^ (CommentText != null ? CommentText.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ Rate;
                hashCode = (hashCode * 397) ^ PublicationDate.GetHashCode();
                return hashCode;
            }
        }
    }
}