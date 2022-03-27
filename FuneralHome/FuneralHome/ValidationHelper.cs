using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FuneralHome
{
    public static class ValidationHelper
    {
        public static bool Validate(Control ctrl, Control [] controlsToValidate)
        {
            bool validate = true;

            foreach(var c in controlsToValidate)
            {
                if(c is ComboBox)
                {
                    if (((ComboBox)c).SelectedItem == null)
                        validate = false;
                    else if (string.IsNullOrEmpty(((ComboBox)c).SelectedItem.ToString()))
                        validate = false;
                }
                else if(c is TextBox)
                {
                    if (string.IsNullOrEmpty(((TextBox)c).Text))
                        validate = false;
                }
            }

            if (!validate)
                MessageBox.Show("Uzupełnij wszystkie pola!");

            return validate;
        }

        public static bool ValidateError()
        {
            MessageBox.Show("Uzupełnij wszystkie pola!");

            return false;
        }
    }
}
