using System;
using System.Collections.Generic;
using System.Text;

namespace Take5.Services.Models
{
    public class CommanData
    {
        public static string DangerIconFolder = "images/dangerIcons/";
        public static string NotificationIconFolder = "https://localhost:44321/images/notificationIcons/";
        public static string UploadMainFolder = "wwwroot/";
        public static int unPackgingTime = 2;

        public static int NormalDifferenceBetweenStep1AndStep2 = 120;
        public enum NotificationTypes
        {
            NewTripAssigned =1,
            TripStarted = 2,
            DestinationArrived = 3,
            StepOneCompleted = 4,
            StepTwoRequest = 5,
            StepTwoResponsed = 6,
            StepTwoCompleted = 7,
            TripConverted = 8,
            TripEditing = 9,
            TripCancelling = 10,
            TripCompleted = 11

        };

        public enum RequestStatus
        {
            Pending = 1,
            Approved = 2,
            Cancelled = 3
        };

        public enum Take5Status
        {
            NotStarted =1,
            StepOneCompletedOnly = 2,
            StepTwoRequested= 3,
            StepTwoCompleted = 4,
            Completed = 5
        };

        public enum TripStatus
        {
            Pending = 1,
            Started = 2,
            DestinationArrived = 3,
            SurveyStepOneCompleted = 4,
            StepTwoRequested = 5,
            StepTwoResponsed = 6,
            SurveyStepTwoCompleted = 7,
            TripConverted = 8,
            TripCompleted = 9
        }

        public enum TripAction
        {
            Assigned = 1,
            Started = 2,
            DestinationArrived = 3,
            SurveyStepOneCompleted = 4,
            StepTwoRequested = 5,
            SurveyStepTwoCompleted = 6
        }

        public enum SurveySteps
        {
            StepOne = 1,
            StepTwo = 2
        }

        public enum WarningTypes
        {
            StepTwoRequestWarning =1,
            StepTwoResponseWarning = 2,
            StepTwoCompletionWarning = 3
        }

    }
}
