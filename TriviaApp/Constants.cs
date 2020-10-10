using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TriviaApp
{
    static class Constants
    {
        public const int GET_TOP_STATISTICS_SUCCESS = 0;
        public const int GET_MY_STATISTICS_SUCCESS = 0;
        public const int GET_RESULTS_SUCCESS = 0;
        public const int GET_RESULTS_ADMIN_STOP = 1;
        public const int LEAVE_GAME_SUCCESS = 0;
        public const int SUBMIT_ANSWER_SUCCESS = 0;
        public const int NEW_ADMIN_SUCCESS = 0;
        public const int NEW_ADMIN_NOT_REAL_ADMIN = 1;
        public const int GET_QUESTION_SUCCESS = 0;
        public const int GET_QUESTION_FINISHED = 1;
        public const int START_GAME_SUCCESS = 0;
        public const int START_GAME_NOT_EXIST = 1;
        public const int GET_ROOM_DETAILS_SUCCESS = 0;
        public const int GET_ROOM_DETAILS_NOT_EXIST = 1;
        public const int CLOSE_ROOM_SUCCESS = 0;
        public const int LEAVE_ROOM_SUCCESS = 0;
        public const int EDIT_ROOM_SUCCESS = 0;
        public const int EDIT_ROOM_NOT_EXIST = 1;
        public const int JOIN_ROOM_SUCCESS = 0;
        public const int JOIN_ROOM_MAXIMUM_USERS_IN_ROOM = 1;
        public const int JOIN_ROOM_NOT_EXIST = 2;
        public const int CREATE_ROOM_SUCCESS = 0;
        public const int CREATE_ROOM_MAXIMUM_ROOMS_IN_SERVER = 1;
        public const int LOGOUT_SUCCESS = 0;
        public const int LOGIN_SUCCESS = 0;
        public const int LOGIN_INCORRECT_PASSWORD = 1;
        public const int LOGIN_USERNAME_NOT_EXIST = 2;
        public const int LOGIN_UNEXPECTED_ERR = 3;
        public const int LOGIN_ALREADY_ONLINE = 4;
        public const int SIGNUP_SUCCESS = 0;
        public const int SIGNUP_EMAIL_OR_USERNAME_EXIST = 1;
        public const int SIGNUP_UNEXPECTED_ERR = 2;
        public const int SIGNUP_REQUEST_CODE = 0;
        public const int LOGIN_REQUEST_CODE = 1;
        public const int SIGNOUT_REQUEST_CODE = 6;
        public const int CREATE_ROOM_REQUEST_CODE = 2;
        public const int GET_ROOM_DETAILS_REQUEST = 4;
        public const int GET_ROOMS_ID_REQUEST_CODE = 7;
        public const int JOIN_ROOM_REQUEST = 3;
        public const int GET_MY_STATISTICS_REQUEST = 5;
        public const int EDIT_ROOM_REQUEST = 8;
        public const int CLOSE_ROOM_REQUEST = 9;
        public const int START_GAME_REQUEST = 10;
        public const int LEAVE_ROOM_REQUEST = 11;
        public const int GET_QUESTION_REQUEST = 12;
        public const int SUBMIT_ANSWER_REQUEST = 13;
        public const int GET_RESULTS_REQUEST = 14;
        public const int LEAVE_GAME_REQUEST = 15;
        public const int NEW_ADMIN_REQUEST = 16;
        public const int GET_TOP_STATISTICS_REQUEST = 17;



    };
    public struct ResponseInfo
    {
        public int code;
        public Byte[] buffer;
    };
    public struct LoginRequest
    {
        public string username;
        public string password;
    };

    public struct LoginResponse
    {
        public int status;
    };
    public struct SignupRequest
    {
        public string username;
        public string password;
        public string email;
    };
    public struct SignupResponse
    {
        public int status;
    };
    public struct SignoutRequest
    {

    };
    public struct GetRoomsIdRequest
    {

    };
    public struct GetRoomsIdResponse
    {
        public int status;
        public int[] ids;
    };
    public struct CreateRoomRequest
    {
        public string roomName;
        public int maxUsers;
        public int questionCount;
        public int answerTimeout;
    };
    public struct CreateRoomResponse
    {
        public int status;
    };
    public struct GetRoomDetailsResponse
    {
        public bool isActive { get; set; }
        public int status { get; set; }
        public int id { get; set; }
        public string admin { get; set; }
        public string[] players { get; set; }
        public int maxPlayers { get; set; }
        public int questionsCount { get; set; }
        public int secondsForQuestions { get; set; }
        public string name { get; set; }
        public string playersAmount { get; set; }
    };
    public struct GetRoomDetailsRequest
    {
        public int roomId;
    };
    public struct JoinRoomRequest
    {
        public int roomId;
    };
    struct NewAdminResponse
    {
        public int status;
    };
    public struct JoinRoomResponse
    {
        public int status;
    };
    public struct CloseRoomResponse
    {
        public int status;
    };
    public struct LeaveRoomResponse
    {
        public int status;
    };
    struct StartGameResponse
    {
        public int status;
    };
    struct EditRoomRequest
    {
        public string roomName;
        public int maxUsers;
        public int questionCount;
        public int answerTimeout;
    };
    struct LeaveRoomRequest
    {

    };
    struct CloseRoomRequest
    {

    };
    struct StartGameRequest
    {

    };
    struct EditRoomResponse
    {
        public int status;
    };
    struct GetQuestionRequest
    {

    };
    struct SubmitQuestionRequest
    {
        public int answerId;
    };
    struct LeaveGameRequest
    {

    };
    struct GetResultsRequest
    {
    };
    struct NewAdminRequest
    {

    };
    struct GetQuestionResponse
    {
        public int status;
        public string question;
        public string[] answers;
        public int currentQuestion;
        public int totalQuestions;
        public int questionTime;
    };
    struct SubmitQuestionResponse
    {
        public int status;
        public int correctAnswerId;
    };
    struct LeaveGameResponse
    {
        public int status;
    };
    struct GetResultsResponse
    {
        public int status;
        public PlayerResult[] results;
    };
    struct GetMyStatisticsResponse
    {
        public int status;
        public float averageAnswerTime;
        public int correctAnswers;
        public int wrongAnswers;
        public int gamesPlayed;
    };
    struct GetMyStatisticsRequest
    {
        
    };
    struct GetTopStatisticsRequest
    {

    };
    struct GetTopStatisticsResponse
    {
        public int status;
        public PlayerStatistics[] topPlayerStatistics;
    };
    struct PlayerStatistics
    {
        public float averageAnswerTime { get; set; }
        public int correctAnswers { get; set; }
        public int wrongAnswers { get; set; }
        public int gamesPlayed { get; set; }
        public string username { get; set; }
    };
    struct PlayerResult
    {
        public string username { get; set; }
        public int correctAnswerCount { get; set; }
        public int wrongAnswerCount { get; set; }
        public float averageAnswerTime { get; set; }
    };

}

