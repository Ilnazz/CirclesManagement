using LiveChartsCore.SkiaSharpView;
using LiveChartsCore;
using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using CirclesManagement.Components;
using LiveChartsCore.SkiaSharpView.Painting;
using Newtonsoft.Json.Linq;
using SkiaSharp;

namespace CirclesManagement.Pages
{
    /// <summary>
    /// Логика взаимодействия для ReportsPage.xaml
    /// </summary>
    public partial class ReportsPage : EntityPage
    {
        public IEnumerable<ReportType> ReportTypes => Enum.GetValues(typeof(ReportType)).Cast<ReportType>();

        private ReportType _selectedReportType = ReportType.LessonCountByWeekDay;
        public ReportType SelectedReportType
        {
            get => _selectedReportType;
            set
            {
                _selectedReportType = value;
                Generate();
            }
        }

        public ReportsPage()
        {
            InitializeComponent();

            Generate();

            DataContext = this;

            EH = new EntityHelper
            {
                Builder = () => new object(),

                IsBlank = (obj) => true,

                Validator = (obj) => (true, ""),

                Comparer = (obj1, obj2) => (true, ""),

                SearchTextMatcher = (obj, searchText) => false,

                Deleter = (obj) => (true, ""),

                IsDeleted = (obj) => false,

                SavePreparator = (obj) => { }
            };

            HasAddFunction = false;
            HasDeleteFunction = false;
            HasEditFunction = false;
            HasCreateLessonFunction = false;

            EntitiesSource = new ObservableCollection<object>();
        }

        private void Generate()
        {
            ISeries series = null;
            Axis axis = null;

            if (_selectedReportType == ReportType.LessonCountByWeekDay)
            {
                var lessonCountsByWeekDay = new Dictionary<WeekDay, int>();
                foreach (var tt in App.DB.Timetables.ToList())
                {
                    var wd = tt.WeekDay;
                    if (!lessonCountsByWeekDay.ContainsKey(wd))
                        lessonCountsByWeekDay[wd] = 1;
                    else
                        lessonCountsByWeekDay[wd]++;
                }

                series = new ColumnSeries<int>
                {
                    Values = lessonCountsByWeekDay.Values
                };

                axis = new Axis
                {
                    Labels = App.DB.WeekDays.ToList().Select(wd => wd.Title).ToList(),
                    LabelsRotation = 0,
                    SeparatorsPaint = new SolidColorPaint(new SKColor(200, 200, 200)),
                    SeparatorsAtCenter = false,
                    TicksPaint = new SolidColorPaint(new SKColor(35, 35, 35)),
                    TicksAtCenter = true,
                    // By default the axis tries to optimize the number of 
                    // labels to fit the available space, 
                    // when you need to force the axis to show all the labels then you must: 
                    ForceStepToMin = true,
                    MinStep = 1
                };
            }
            else if (_selectedReportType == ReportType.PupilCountByGroup)
            {
                var pupilCountByGroup = new Dictionary<Group, int>();
                foreach (var group in App.DB.Groups.ToList())
                    pupilCountByGroup[group] = group.Group_Pupil.Count;

                series = new ColumnSeries<int>
                {
                    Values = pupilCountByGroup.Values
                };

                axis = new Axis
                {
                    Labels = App.DB.Groups.ToList().Select(group => group.Circle.Title).ToList(),
                    LabelsRotation = 0,
                    SeparatorsPaint = new SolidColorPaint(new SKColor(200, 200, 200)),
                    SeparatorsAtCenter = false,
                    TicksPaint = new SolidColorPaint(new SKColor(35, 35, 35)),
                    TicksAtCenter = true,
                    // By default the axis tries to optimize the number of 
                    // labels to fit the available space, 
                    // when you need to force the axis to show all the labels then you must: 
                    ForceStepToMin = true,
                    MinStep = 1
                };
            }
            else if (_selectedReportType == ReportType.CirclesAttendance)
            {
                var percentByLesson = new Dictionary<Lesson, double>();
                foreach (var lesson in App.DB.Lessons.ToList())
                {
                    var group = lesson.Timetable.Group;
                    var pupilCountInGroup = group.Group_Pupil.Where(gp => gp.IsAttending).Count();
                    var pupilCountOnLesson = lesson.Lesson_Pupil.Where(lp => lp.WasInClass).Count();
                    var percent = (double)pupilCountOnLesson / pupilCountInGroup;

                    percentByLesson[lesson] = percent;
                }

                series = new ColumnSeries<double>
                {
                    Values = percentByLesson.Values,
                    DataLabelsFormatter = (p) => $"{p.Model:p}"
                };

                axis = new Axis
                {
                    Labels = App.DB.Lessons.ToList().Select(l => $"{l.Date:d}, {l.Timetable.Group.Circle.Title}").ToList(),
                    LabelsRotation = 0,
                    SeparatorsPaint = new SolidColorPaint(new SKColor(200, 200, 200)),
                    SeparatorsAtCenter = false,
                    TicksPaint = new SolidColorPaint(new SKColor(35, 35, 35)),
                    TicksAtCenter = true,
                    // By default the axis tries to optimize the number of 
                    // labels to fit the available space, 
                    // when you need to force the axis to show all the labels then you must: 
                    ForceStepToMin = true,
                    MinStep = 1
                };
            }

            Chart.Series = new ISeries[] { series };
            Chart.XAxes = new Axis[] { axis };
        }
    }

    public enum ReportType
    {
        LessonCountByWeekDay,
        PupilCountByGroup,
        CirclesAttendance
    }
}