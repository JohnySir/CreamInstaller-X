using System;
using System.Windows.Forms;
using CreamInstaller.Components;

namespace CreamInstaller.Forms;

internal sealed partial class ArchiveChoiceForm : CustomForm
{
    internal ArchiveChoiceForm(IWin32Window owner) : base(owner) => InitializeComponent();

    internal static DialogResult Ask(IWin32Window owner)
    {
        using ArchiveChoiceForm form = new(owner);
        return form.ShowDialog();
    }
}
