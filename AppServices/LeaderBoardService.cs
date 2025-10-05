using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TDEduEnglish.DomainModels;
using TDEduEnglish.Repository;

namespace TDEduEnglish.Services {
    public class LeaderBoardService : ILeaderBoardService {
        private readonly IUserAttemptRepository _attemptRepo;
        private readonly IUserScoreRepository _scoreRepo;

        public LeaderBoardService(IUserAttemptRepository attemptRepo, IUserScoreRepository scoreRepo) {
            _attemptRepo = attemptRepo;
            _scoreRepo = scoreRepo;
        }

        public async Task<bool> SubmitAttemptAsync(int userId, int exerciseId, string exerciseType, int newScore, int maxScore) {
            var attemptsToday = await _attemptRepo.GetAttemptsToday(userId, exerciseId, exerciseType);

            if (attemptsToday.Any(a => a.Score == maxScore))
                return false;

            int bestPrevScore = attemptsToday.Any() ? attemptsToday.Max(a => a.Score) : 0;
            int deltaScore = Math.Max(newScore - bestPrevScore, 0);

            var attempt = new UserAttempt {
                UserId = userId,
                ExerciseId = exerciseId,
                ExerciseType = exerciseType,
                Score = newScore,
                MaxScore = maxScore,
                AttemptDate = DateTime.Now
            };

            await _attemptRepo.Add(attempt);

            var userScore = await _scoreRepo.GetByUserId(userId);
            if (userScore == null) {
                userScore = new UserScore {
                    UserId = userId,
                    AllTimeScore = deltaScore,
                    ThisMonthScore = deltaScore,
                    ThisWeekScore = deltaScore
                };
                await _scoreRepo.Add(userScore);
            }
            else {
                userScore.AllTimeScore += deltaScore;
                userScore.ThisMonthScore += deltaScore;
                userScore.ThisWeekScore += deltaScore;
                await _scoreRepo.Update(userScore);
            }
            return true;
        }
    }
}
