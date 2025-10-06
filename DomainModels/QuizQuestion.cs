using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace TDEduEnglish.DomainModels {
    public class QuizQuestion : INotifyPropertyChanged {
        [Key]
        public int QuizQuestionId { get; set; }
        public int QuizId { get; set; }
        [ForeignKey(nameof(QuizId))]
        public Quiz Quiz { get; set; }
        public string QuestionText { get; set; } = "";
        public string CorrectAnswer { get; set; } = "";
        public string Explaination { get; set; } = "";
        public string Option1 { get; set; } = "";
        public string Option2 { get; set; } = "";
        public string Option3 { get; set; } = "";
        public string Option4 { get; set; } = "";

        private bool _isOption1Selected = false;
        public bool IsOption1Selected {
            get => _isOption1Selected;
            set {
                if (_isOption1Selected != value) {
                    _isOption1Selected = value;
                    OnPropertyChanged();
                }
            }
        }

        private bool _isOption2Selected = false;
        public bool IsOption2Selected {
            get => _isOption2Selected;
            set {
                if (_isOption2Selected != value) {
                    _isOption2Selected = value;
                    OnPropertyChanged();
                }
            }
        }

        private bool _isOption3Selected = false;
        public bool IsOption3Selected {
            get => _isOption3Selected;
            set {
                if (_isOption3Selected != value) {
                    _isOption3Selected = value;
                    OnPropertyChanged();
                }
            }
        }

        private bool _isOption4Selected = false;
        public bool IsOption4Selected {
            get => _isOption4Selected;
            set {
                if (_isOption4Selected != value) {
                    _isOption4Selected = value;
                    OnPropertyChanged();
                }
            }
        }

        private bool _isFlagged = false;
        public bool IsFlagged {
            get => _isFlagged;
            set {
                if (_isFlagged != value) {
                    _isFlagged = value;
                    OnPropertyChanged();
                }
            }
        }

        private int _quizQuestionNumber = 0;
        public int QuizQuestionNumber {
            get => _quizQuestionNumber;
            set {
                if (_quizQuestionNumber != value) {
                    _quizQuestionNumber = value;
                    OnPropertyChanged();
                }
            }
        }

        private bool _isCurrentQuestion = false;
        public bool IsCurrentQuestion {
            get => _isCurrentQuestion;
            set {
                if (_isCurrentQuestion != value) {
                    _isCurrentQuestion = value;
                    OnPropertyChanged();
                }
            }
        }

        private bool _isAnswered = false;
        public bool IsAnswered {
            get => _isAnswered;
            set {
                if (_isAnswered != value) {
                    _isAnswered = value;
                    OnPropertyChanged();
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}