using CultusBot.Models.Static;

namespace CultusBot.Models
{
    public class Book
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public int LikesCount { get; set; }

        public string Description { get; set; }

        public BookUser Author { get; set; }

        public bool IsTranslation { get; set; }

        public string OriginalLink { get; set; }

        public BookRating Rating { get; set; }
        public string RatingText { get {
                switch (Rating)
                {
                    case BookRating.G:
                        return "Загальний (G) | 0+";
                    case BookRating.M:
                        return "Дорослий (M) | 15+";
                    case BookRating.R:
                        return "Обмежений (R) | 18+";
                    case BookRating.RE:
                        return "Екстремальний (RE) | 21+";
                    default:
                        return "";
                }
            } 
        }

        public BookSize? BookSize { get; set; }
        public string BookSizeText
        {
            get
            {
                switch (BookSize)
                {
                    case Static.BookSize.Micro:
                        return "Мікро";
                    case Static.BookSize.Mini:
                        return "Міні";
                    case Static.BookSize.Midi:
                        return "Міді";
                    case Static.BookSize.Maxi:
                        return "Максі";
                    case Static.BookSize.Epic:
                        return "Епічний";
                    case Static.BookSize.Legend:
                        return "Легендарний";
                    default:
                        return "";
                }
            }
        }

        public BookSize? ExpectedBookSize { get; set; }

        public string ExpectedBookSizeText
        {
            get
            {
                switch(ExpectedBookSize)
                {
                    case Static.BookSize.Micro:
                        return "Мікро";
                    case Static.BookSize.Mini:
                        return "Міні";
                    case Static.BookSize.Midi:
                        return "Міді";
                    case Static.BookSize.Maxi:
                        return "Максі";
                    case Static.BookSize.Epic:
                        return "Епічний";
                    case Static.BookSize.Legend:
                        return "Легендарний";
                    default:
                        return "";
                }
            }
        }

        public int PagesCount { get; set; }

        public BookStatus BookStatus { get; set; }
        public string BookStatusText
        {
            get
            {
                switch(BookStatus)
                {
                    case BookStatus.InProgress:
                        return "В процесі";
                    case BookStatus.Completed:
                        return "Завершений";
                    case BookStatus.OnHold:
                        return "Заморожений";
                    case BookStatus.Dropped:
                        return "Закинутий";
                    default:
                        return "";
                }
            }
        }

        public List<BookPair> Pairings { get; set; }

        public List<BookTag> Tags { get; set; }
    }
}
