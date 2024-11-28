using System.ComponentModel.DataAnnotations;

namespace LibraryCW.Models
{
    public class RoomDisplayModel
    {
        public string RoomId { get; set; }
        public string RoomName { get; set; }
        public List<ShelfDisplayModel> Shelves { get; set; }
    }

    public class RowDisplayModel
    {
        public string RowId { get; set; }
        public int RowNumber { get; set; }
        public List<BookDisplayModel> Books { get; set; }

    }

    public class ShelfDisplayModel
    {
        public string ShelfId { get; set; }
        public List<string> ShelfGenres { get; set; }
        public List<RowDisplayModel> Rows { get; set; }
    }

    public class BookDisplayModel
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Picture { get; set; }
        public int BookNumberInRow { get; set; }
    }

    public class RoomUpdateModel
    {
        public Guid Id { get; set; }
        [Required]
        public string Name { get; set; }
    }

    public class ShelfCreateModel
    {
        public Guid RoomId { get; set; }
        public List<Guid> Genres { get; set; }
    }

    public class ShelfUpdateModel
    {
        public Guid ShelfId { get; set; }
        public List<Guid> Genres { get; set; }
    }

}
