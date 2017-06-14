using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SportApp.Models
{
    public class Gym : IIdentifiable
    {
        public int Id { get; set; }

        [Required(AllowEmptyStrings = false), Column(TypeName = "varchar(255)"), MaxLength(255)]
        public string GymName { get; set; }

        [Required]
        public int GymRate { get; set; }

        [Required(AllowEmptyStrings = false)]
        public string GymLocation { get; set; }

        [Required(AllowEmptyStrings = false)]
        public string GoogleLocation { get; set; }
    
        public int? MbrshipPrice { get; set; }

        public int? GymArea { get; set; }

        public int? FoundYear { get; set; }

        public string Facilities { get; set; } // !!!

        public string Url { get; set; }

        [Required(AllowEmptyStrings = false), Column(TypeName = "varchar(10000)"), MaxLength(10000)]
        public string Description { get; set; }

        public string GymImgUrl { get; set; }

        public List<Comment> Comments { get; set; }

        public double Longitude { get; set; }

        public double Latitude { get; set; }

        protected bool Equals(Gym other)
        {
            return Id == other.Id &&
                string.Equals(GymName, other.GymName) && 
                GymRate == other.GymRate && 
                string.Equals(GymLocation, other.GymLocation) &&
                string.Equals(GoogleLocation, other.GoogleLocation) && 
                MbrshipPrice == other.MbrshipPrice && 
                GymArea == other.GymArea && 
                FoundYear == other.FoundYear && 
                string.Equals(Facilities, other.Facilities) && 
                string.Equals(Url, other.Url) && 
                string.Equals(Description, other.Description) && 
                string.Equals(GymImgUrl, other.GymImgUrl) && 
                Equals(Comments, other.Comments);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Gym)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = Id;
                hashCode = (hashCode * 397) ^ (GymName != null ? GymName.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ GymRate;
                hashCode = (hashCode * 397) ^ (GymLocation != null ? GymLocation.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (GoogleLocation != null ? GoogleLocation.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (int)MbrshipPrice;
                hashCode = (hashCode * 397) ^ (int)GymArea;
                hashCode = (hashCode * 397) ^ (int)FoundYear;
                hashCode = (hashCode * 397) ^ (Facilities != null ? Facilities.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (Url != null ? Url.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (Description != null ? Description.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (GymImgUrl != null ? GymImgUrl.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (Comments != null ? Comments.GetHashCode() : 0);
                return hashCode;
            }
        }
    }
}