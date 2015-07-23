using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace KitsTajmMobile.ViewModels
{
    public class ReportsViewModel
    {
        public Position Text1 { get; private set; }
        public Position Text2 { get; private set; }
        public Position Button { get; private set; }

        public ReportsViewModel()
        {
            this.Text1 = new Position
            {
                Column = 0,
                Row = 0
            };
            this.Text2 = new Position
            {
                Column = 1,
                Row = 0
            };
            this.Button = new Position
            {
                Column = 0,
                Row = 1
            };
        }

        public class Position : INotifyPropertyChanged
        {
            private int _row;
            private int _column;

            public int Column
            {
                get
                {
                    return this._column;
                }
                set
                {
                    if (this._column != value)
                    {
                        this._column = value;

                        NotifyPropertyChanged();
                    }
                }
            }
            public int Row
            {
                get
                {
                    return this._row;
                }
                set
                {
                    if (this._row != value)
                    {
                        this._row = value;

                        NotifyPropertyChanged();
                    }
                }
            }

            public event PropertyChangedEventHandler PropertyChanged;

            private void NotifyPropertyChanged([CallerMemberName]string propertyName = "")
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
