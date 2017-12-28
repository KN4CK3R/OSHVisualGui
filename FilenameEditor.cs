using System;
using System.ComponentModel;
using System.Drawing.Design;
using System.Windows.Forms;

namespace OSHVisualGui
{
	public class FilenameEditor : UITypeEditor
	{
		public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
		{
			return context?.Instance != null ? UITypeEditorEditStyle.Modal : UITypeEditorEditStyle.None;
		}

		[RefreshProperties(RefreshProperties.All)]
		public override object EditValue(ITypeDescriptorContext context, System.IServiceProvider provider, object value)
		{
			if (context == null || provider == null || context.Instance == null)
			{
				return base.EditValue(provider, value);
			}

			FileDialog fileDlg;
			if (context.PropertyDescriptor.Attributes[typeof(SaveFileAttribute)] == null)
			{
				fileDlg = new OpenFileDialog();
			}
			else
			{
				fileDlg = new SaveFileDialog();
			}
			fileDlg.Title = "Select " + context.PropertyDescriptor.DisplayName;
			fileDlg.FileName = value as string;

			if (context.PropertyDescriptor.Attributes[typeof(FileDialogFilterAttribute)] is FileDialogFilterAttribute filterAtt)
			{
				fileDlg.Filter = filterAtt.Filter;
			}
			if (fileDlg.ShowDialog() == DialogResult.OK)
			{
				value = fileDlg.FileName;
			}
			fileDlg.Dispose();
			return value;
		}
	}

	[AttributeUsage(AttributeTargets.Property)]
	public class FileDialogFilterAttribute : Attribute
	{
		//"Text files (*.txt)|*.txt|All files (*.*)|*.*"
		public string Filter { get; }

		public FileDialogFilterAttribute(string filter)
		{
			Filter = filter;
		}
	}

	[AttributeUsage(AttributeTargets.Property)]
	public class SaveFileAttribute : Attribute
	{

	}
}
