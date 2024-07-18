using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportFrontPage
{
    public enum ReportStatuses
    {
        UnderReview,
        Reviewed
    }
    public class ReportProfile : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        #region Properties
        public int ID { get; set; }
        public string ReportName { get; set; }
        public string Prefix { get; set; }
        public string OperatorName { get; set; }
        public bool IsVerified { get; set; }
        #endregion

        [NotMapped]
        public int DefectCount { get; set; }

        [NotMapped]
        public bool IsSelected
        {
            get => isSelected;
            set
            {
                isSelected = value;
                OnPropertyChanged(nameof(IsSelected));
            }
        }

        [NotMapped]
        public ReportStatuses ReportStatus
        {
            get => reportStatus;
            set
            {
                reportStatus = value;
                OnPropertyChanged(nameof(ReportStatus));
            }
        }

        [NotMapped]
        public string DefectCountString
        {
            get
            {
                return "Barcode Count: " + DefectCount;
            }
        }

        [NotMapped]
        public string OperatorNameString
        {
            get
            {
                return "Operator Name: " + OperatorName;
            }
        }

        [NotMapped]
        public string PrefixString
        {
            get
            {
                return "Prefix: " + Prefix;
            }
        }

        [NotMapped]
        private bool isSelected = false;

        [NotMapped]
        private ReportStatuses reportStatus = ReportStatuses.UnderReview;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

}
