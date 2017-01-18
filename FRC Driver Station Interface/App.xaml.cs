using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Shell;

namespace FRC_Driver_Station_Interface
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static readonly DependencyProperty StatusProperty = DependencyProperty.RegisterAttached("Status", typeof(string), typeof(App), new PropertyMetadata("Ready"));

        public static readonly DependencyProperty ProgressProperty = DependencyProperty.RegisterAttached("Progress", typeof(double?), typeof(App), new PropertyMetadata(null, progressChanged));

        private static readonly DependencyPropertyKey ProgressValuePropertyKey = DependencyProperty.RegisterAttachedReadOnly("ProgressValue", typeof(double), typeof(App), new PropertyMetadata(0.0));

        public static readonly DependencyProperty ProgressValueProperty = ProgressValuePropertyKey.DependencyProperty;

        private static readonly DependencyPropertyKey ProgressIndeterminatePropertyKey = DependencyProperty.RegisterAttachedReadOnly("ProgressIndeterminate", typeof(bool), typeof(App), new PropertyMetadata(false));

        public static readonly DependencyProperty ProgressIndeterminateProperty = ProgressIndeterminatePropertyKey.DependencyProperty;

        private static readonly DependencyPropertyKey ProgressVisibilityPropertyKey = DependencyProperty.RegisterAttachedReadOnly("ProgressVisibility", typeof(Visibility), typeof(App), new PropertyMetadata(Visibility.Collapsed));

        public static readonly DependencyProperty ProgressVisibilityProperty = ProgressVisibilityPropertyKey.DependencyProperty;

        private static readonly DependencyPropertyKey ProgressStatePropertyKey = DependencyProperty.RegisterAttachedReadOnly("ProgressState", typeof(TaskbarItemProgressState), typeof(App), new PropertyMetadata(TaskbarItemProgressState.None));

        public static readonly DependencyProperty ProgressStateProperty = ProgressStatePropertyKey.DependencyProperty;

        public static string GetStatus(DependencyObject target) => target.GetValue(StatusProperty) as string;

        public static void SetStatus(DependencyObject target, string value) => target.SetValue(StatusProperty, value);

        public static double? GetProgress(DependencyObject target) => target.GetValue(ProgressProperty) as double?;

        public static void SetProgress(DependencyObject target, double? value) => target.SetValue(ProgressProperty, value);

        public static double GetProgressValue(DependencyObject target) => (double)target.GetValue(ProgressValueProperty);

        public static bool GetProgressIndeterminate(DependencyObject target) => (bool)target.GetValue(ProgressIndeterminateProperty);

        public static Visibility GetProgressVisibility(DependencyObject target) => (Visibility)target.GetValue(ProgressVisibilityProperty);

        public static TaskbarItemProgressState GetProgressState(DependencyObject target) => (TaskbarItemProgressState)target.GetValue(ProgressStateProperty);

        private static void progressChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue is double v)
            {
                o.SetValue(ProgressVisibilityPropertyKey, Visibility.Visible);
                if (Double.IsNaN(v))
                {
                    o.SetValue(ProgressIndeterminatePropertyKey, true);
                    o.SetValue(ProgressStatePropertyKey, TaskbarItemProgressState.Indeterminate);
                }
                else
                {
                    o.SetValue(ProgressIndeterminatePropertyKey, false);
                    o.SetValue(ProgressStatePropertyKey, TaskbarItemProgressState.Normal);
                    o.SetValue(ProgressValuePropertyKey, v);
                }
            }
            else
            {
                o.SetValue(ProgressVisibilityPropertyKey, Visibility.Collapsed);
                o.SetValue(ProgressStatePropertyKey, TaskbarItemProgressState.None);
            }
        }
    }
}
