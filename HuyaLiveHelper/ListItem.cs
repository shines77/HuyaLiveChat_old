using System;
using System.Text;

namespace HuyaLiveHelper
{
    /// <summary>
    /// 选择项类，用于ComboBox或者ListBox添加项
    /// </summary>
    public class ListItem
    {
        private string id = string.Empty;
        private string name = string.Empty;

        public ListItem(string id, string name)
        {
            this.id = id;
            this.name = name;
        }

        public override string ToString()
        {
            return this.name;
        }

        public string Id
        {
            get
            {
                return this.id;
            }
            set
            {
                this.id = value;
            }
        }

        public string Name
        {
            get
            {
                return this.name;
            }
            set
            {
                this.name = value;
            }
        }
    }
}
