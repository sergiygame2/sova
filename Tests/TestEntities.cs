using System;
using System.Collections.Generic;
using System.Text;
using SportApp.Models;

namespace Tests
{
    public class TestEntities
    {
        public static List<Gym> Gyms = new List<Gym>
        {
            new Gym
            {
                Id = 1,
                GymName = "TestGym",
                Description = "Gym description",
                FoundYear = 2014,
                Url = "site",
                GoogleLocation = "location",
                Facilities = "pool",
                GymArea = 100,
                GymImgUrl = "imageurl",
                GymLocation = "Kiev",
                GymRate = 7,
                MbrshipPrice = 1500,
                Comments = new List<Comment>()
            },
            new Gym
            {
                Id = 2,
                GymName = "TestGym2",
                Description = "Gym description",
                FoundYear = 2017,
                Url = "site",
                GoogleLocation = "location",
                Facilities = "pool",
                GymArea = 200,
                GymImgUrl = "imageurl",
                GymLocation = "Kiev",
                GymRate = 8,
                MbrshipPrice = 2500,
                Comments = new List<Comment>()
            },
             new Gym
            {
                GymName = "",
                Description = null,
                FoundYear = 2014,
                Url = "site",
                GoogleLocation = "location",
                Facilities = "pool",
                GymArea = 200,
                GymImgUrl = "imageurl",
                GymLocation = "Kiev",
                GymRate = 9,
                MbrshipPrice = 2500,
                Comments = new List<Comment>()
            }

        };

        public static List<Comment> Comments = new List<Comment>
        {
            new Comment
            {
                Id = 1,
                GymId = 1,
                UserId = "2",
                CommentText = "Some comment text",
                PublicationDate = new DateTime(1997, 8,27),
                Rate = 5

            },
            new Comment
            {
                Id = 2,
                GymId = 2,
                UserId = "3",
                CommentText = "Some comment text2",
                PublicationDate = new DateTime(2014, 6,12),
                Rate = 10

            },
             new Comment
            {
                Id = 3,
                GymId = 2,
                UserId = "4",
                CommentText = null,
                PublicationDate = new DateTime(2014, 6,12),
                Rate = 8

             }
             
        };
     }
}
