using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.Command;

namespace WpfTestProject.Models
{
    public class Info
    {
        public virtual string TemplateKey { get; set; }
        public string Title { get; set; }
        public int No { get; set; }
        public string SubTitle { get; set; }
        public object Tag { get; set; }

        public override string ToString()
        {
            return $"[{No}] {Title}";
        }

       
        public RelayCommand<Info> ConfirmCommand { get; set; }
    }
}
