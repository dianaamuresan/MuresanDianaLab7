using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MuresanDianaLab7
{
    public class ValidationBehaviour : Behavior<Editor>
    {
        protected override void OnAttachedTo(Editor editor)
        {
            editor.TextChanged += OnEntryTextChanged;
            base.OnAttachedTo(editor);
        }

        protected override void OnDetachingFrom(Editor editor)
        {
            editor.TextChanged -= OnEntryTextChanged;
            base.OnDetachingFrom(editor);
        }

        void OnEntryTextChanged(object sender, TextChangedEventArgs e)
        {
            ((Editor)sender).BackgroundColor =
                string.IsNullOrEmpty(e.NewTextValue) ? Color.FromRgba("#AA4A44") : Color.FromRgba("#FFFFFF");
        }
    }
}
