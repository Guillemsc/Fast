using System;
using System.Collections.Generic;

namespace Fast.UI
{
    public class UIController : Fast.IController
    {
        private Form main_form = null;
        private readonly List<Form> sub_forms = new List<Form>();

        private readonly List<Form> to_show = new List<Form>();
        private readonly List<Form> to_hide = new List<Form>();

        public Form MainForm => main_form;
        public IReadOnlyList<Form> SubForms => sub_forms;

        public void SetMainForm(Form form)
        {
            if(main_form != null)
            {
                main_form.Hide();
            }

            if(form == null)
            {
                return;
            }

            main_form = form;

            main_form.Show();
        }

        public void AddSubForm(Form form)
        {
            if (form == null)
            {
                return;
            }

            for(int i = 0; i < sub_forms.Count; ++i)
            {
                if(sub_forms[i] == form)
                {
                    return;
                }
            }

            sub_forms.Add(form);

            form.Show();
        }

        public void RemoveSubForm(Form form)
        {
            if (form == null)
            {
                return;
            }

            form.Hide();

            sub_forms.Remove(form);
        }
    }
}
