using System;
using System.Collections.ObjectModel;
using System.Windows;

namespace CredentialFormatsSample.Models
{
    public class FormatTypesModel : DependencyObject
    {
        public string Name { get; set; }
        public Guid Id { get; set; }

        public ObservableCollection<FieldModel> Fields { get; set; }

        public FormatTypesModel()
        {
            Fields = new ObservableCollection<FieldModel>();
        }
    }

    public class FieldModel
    {
        public string FieldName { get; set; }
        public string FieldValue { get; set; }

        public FieldModel(string fieldName, string fieldValue)
        {
            FieldName = fieldName;
            FieldValue = fieldValue;
        }
    }
}
