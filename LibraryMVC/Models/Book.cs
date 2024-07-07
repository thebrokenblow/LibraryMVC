using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LibraryMVC.Models;

public class Book
{
    public required int Id { get; set; }
    public required string Author { get; set; }
    public required Int16 Year { get; set; }
    public required string Title { get; set; }
    public required string Description { get; set; }

    [Column("table_contents")]
    public required string TableContents { get; set; }
}