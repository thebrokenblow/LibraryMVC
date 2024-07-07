using System.ComponentModel.DataAnnotations.Schema;
using System.Xml;

namespace LibraryMVC.Models;

public class BookView
{
    public required int Id { get; set; }
    public required string Author { get; set; }
    public required Int16 Year { get; set; }
    public required string Title { get; set; }
    public required string Description { get; set; }

    [Column("table_contents")]
    public required List<string> TableContents { get; set; }
    public required XmlDocument TableContentsXML { get; set; }
}