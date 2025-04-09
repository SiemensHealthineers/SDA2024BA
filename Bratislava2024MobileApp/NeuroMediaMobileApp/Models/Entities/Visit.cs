using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CommunityToolkit.Mvvm.ComponentModel;

using NeuroMediaMobileApp.Models.Enums;

namespace NeuroMediaMobileApp.Models.Entities
{
    public partial class Visit : ObservableObject
    {
        [ObservableProperty]
        private int _visitId;

        [ObservableProperty]
        private VisitType _visitType;

        [ObservableProperty]
        private DateTime _dateOfVisit;

        [ObservableProperty]
        private int _patientId;
    }
}
