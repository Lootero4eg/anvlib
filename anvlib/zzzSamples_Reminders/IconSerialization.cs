using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Xml.Serialization;
using System.ComponentModel;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;

namespace anvlib.zzzSamples_Reminders
{
    public sealed class IconSerialization
    {
        private Bitmap _icon;

        public void SetIcon(Bitmap Icon)
        {
            _icon = Icon;
        }

        public Bitmap GetIcon()
        {
            if (_icon != null)
                return _icon;
            else
                return new Bitmap(1, 1);
        }

        [XmlElement("Icon")]
        public byte[] IconData
        {
            get
            { // serialize
                if (_icon == null) return null;
                using (MemoryStream ms = new MemoryStream())
                {
                    _icon.Save(ms, ImageFormat.Png);
                    return ms.ToArray();
                }
            }
            set
            { // deserialize
                if (value == null)
                {
                    _icon = null;
                }
                else
                {
                    using (MemoryStream ms = new MemoryStream(value))
                    {
                        _icon = new Bitmap(ms);
                    }
                }
            }
        }
    }
}
