using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fast.Flow
{
    public class FlowState
    {
        private UI.Form curr_form = null;
        private UI.Form last_curr_form = null;

        private UI.Form curr_sub_form = null;
        private List<UI.Form> curr_sub_forms = new List<UI.Form>();

        public UI.Form CurrForm
        {
            get { return curr_form; }
            set { curr_form = value; }
        }

        public UI.Form LastCurrForm
        {
            get { return last_curr_form; }
            set { last_curr_form = value; }
        }

        public UI.Form CurrSubForm
        {
            get { return curr_sub_form; }
        }

        public List<UI.Form> CurrSubForms
        {
            get { return curr_sub_forms; }
        }

        public void AddAndSetCurrSubForm(UI.Form form)
        {
            bool already_added = false;

            for (int i = 0; i < curr_sub_forms.Count; ++i)
            {
                UI.Form curr_sub_form = curr_sub_forms[i];

                if (curr_sub_form == form)
                {
                    already_added = true;

                    break;
                }
            }

            if (!already_added)
            {
                curr_sub_forms.Add(form);
            }

            curr_sub_form = form;
        }

        public void RemoveCurrSubForm(UI.Form form)
        {
            for (int i = 0; i < curr_sub_forms.Count; ++i)
            {
                UI.Form curr_sub_form = curr_sub_forms[i];

                if (curr_sub_form == form)
                {
                    curr_sub_forms.RemoveAt(i);

                    break;
                }
            }

            if (curr_sub_form == form)
            {
                curr_sub_form = null;
            }
        }

        public void ClearCurrSubForms()
        {
            curr_sub_forms.Clear();

            curr_sub_form = null;
        }
    }
}
