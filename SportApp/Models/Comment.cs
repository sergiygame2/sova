using System;

namespace SportApp.Models{
public class Comments{
public string GymId { get; set; }
public int UserId { get; set; }
public string Comment { get; set; }
public int Rate { get; set; }
public DateTime PublicationDate { get; set; }

}
}