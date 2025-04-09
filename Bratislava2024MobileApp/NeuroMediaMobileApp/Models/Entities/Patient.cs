using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CommunityToolkit.Mvvm.ComponentModel;

using NeuroMediaMobileApp.Helpers;
using NeuroMediaMobileApp.Models.Enums;

namespace NeuroMediaMobileApp.Models.Entities
{
    public partial class Patient : ObservableObject
    {
        [ObservableProperty]
        private int _id;

        [ObservableProperty]
        private string _name;

        [ObservableProperty]
        private string _surname;

        [ObservableProperty]
        private DateTime _dateOfBirth;

        [ObservableProperty]
        private Sex _sex;

        [ObservableProperty]
        private Diagnosis _diagnosis;

        [ObservableProperty]
        private DateTime _dateOfDiagnosis;

        [ObservableProperty]
        private string _email;

        [ObservableProperty]
        private string _phoneNumber;

        [ObservableProperty]
        private EducationLevel _highestEducation;

        [ObservableProperty]
        private EmploymentStatus _employmentStatus;

        [ObservableProperty]
        private bool _rapExamination;

        [ObservableProperty]
        private bool _previousBotulinumToxinApplication;

        [ObservableProperty]
        private string _pseudonym;

        [ObservableProperty]
        private string _userId;

        [ObservableProperty]
        private string _tenantId;

        [ObservableProperty]
        private bool _isActive;

        public string DisplaySex => Sex.GetDisplayName();
        public string DisplayHighestEducation => HighestEducation.GetDisplayName();
        public string DisplayEmploymentStatus => EmploymentStatus.GetDisplayName();
        public string DisplayDiagnosis => Diagnosis.GetDisplayName();
        public string DisplayDiseaseDuration => CalculateDisease.CalculateDiseaseDuration(DateOfDiagnosis);

        public string FullName => $"{Name} {Surname}";
    }

}
