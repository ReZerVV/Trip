using System;
using System.Collections.ObjectModel;
using System.Linq;
using Trip.App.ViewModels;
using Trip.Services;
using Trip.Services.Dtos.Reviews;

namespace Trip.App.Commnds
{
    public class AddReviewCommand : CommandBase
    {
        private readonly HistoryViewModel _historyViewModel;
        private readonly IReviewService _reviewService;

        public AddReviewCommand(HistoryViewModel historyViewModel, IReviewService reviewService)
        {
            _historyViewModel = historyViewModel;
            _reviewService = reviewService;
        }

        public override void Execute(object? parameter)
        {
            try
            {
                var result =_reviewService.CreateReview(new CreateReviewDto 
                {
                    TripId = _historyViewModel.TripId,
                    ReviewerId = _historyViewModel.AppUserId,
                    Grade = _historyViewModel.Grade,
                    Description = _historyViewModel.Description,
                });
                if (result.Succeed)
                {
                    _historyViewModel.Description = string.Empty;
                    _historyViewModel.Grade = 0;
                    _historyViewModel.Reviews = new ObservableCollection<ReviewDto>(
                        _reviewService.GetReviewsByTripId(_historyViewModel.TripId).Data.ToList()
                    );
                }
            }
            catch (Exception ex) 
            {

            }
        }
    }
}
