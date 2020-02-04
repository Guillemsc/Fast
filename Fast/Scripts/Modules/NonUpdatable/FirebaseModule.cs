using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

// To use this, go to Unity Project Settings -> Other settings -> Scripting Define Simbols
// and add the preprocessor directives when necessary:
// (keep in mind you have to do this for every platform)
// USING_FIREBASE_AUTH
// USING_FIREBASE_ANALYTICS
// USING_GOOGLE_PLAY_SERVICES

// You can download old versions of firebase using (changing the {version}): https://dl.google.com/firebase/sdk/unity/firebase_unity_sdk_{version}.zip

namespace Fast.Modules
{
    public enum FirebaseAuthType
    {
        AS_GUEST,
        EMAIL_PASSWORD,
        GOOGLE_PLAY_GAMES,
        APPLE_GAME_CENTER,
    }

    public class FirebaseLoginObj
    {
        private FirebaseAuthType auth_type = new FirebaseAuthType();
        private string firebase_user_id = "";

        public FirebaseLoginObj(FirebaseAuthType auth_type, string firebase_user_id)
        {
            this.auth_type = auth_type;
            this.firebase_user_id = firebase_user_id;
        }

        public string FirebaseUserId
        {
            get { return firebase_user_id; }
        }

        public FirebaseAuthType AuthType
        {
            get { return auth_type; }
        }
    }

    class FirebaseModule : Module
    {
        private FirebaseAuthType auth_to_use = new FirebaseAuthType();

#if USING_FIREBASE_AUTH && !UNITY_WEBGL

        private Firebase.FirebaseApp app = null;

        private Firebase.Auth.FirebaseAuth auth = null;

        private Firebase.Auth.FirebaseUser user = null;

#endif

        public override void Awake()
        {
            ChooseAuthType();
        }

        public override void Start()
        {

#if UNITY_ANDROID && USING_GOOGLE_PLAY_SERVICES

            GooglePlayGames.BasicApi.PlayGamesClientConfiguration config = new GooglePlayGames.BasicApi.PlayGamesClientConfiguration.Builder()
            .Build();

            GooglePlayGames.PlayGamesPlatform.InitializeInstance(config);
            GooglePlayGames.PlayGamesPlatform.DebugLogEnabled = true;

            GooglePlayGames.PlayGamesPlatform.Activate();

#endif

#if USING_FIREBASE_AUTH && !UNITY_WEBGL

            Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task => {
                var dependencyStatus = task.Result;

                if (dependencyStatus == Firebase.DependencyStatus.Available)
                {
                    Firebase.FirebaseApp app = Firebase.FirebaseApp.DefaultInstance;

                    auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
                }
                else
                {
                    UnityEngine.Debug.LogError(System.String.Format(
                      "Could not resolve all Firebase dependencies: {0}", dependencyStatus));
                }
            });

#endif

        }

        private void ChooseAuthType()
        {
            auth_to_use = FirebaseAuthType.EMAIL_PASSWORD;

#if UNITY_ANDROID && !UNITY_EDITOR

            auth_to_use = FirebaseAuthType.GOOGLE_PLAY_GAMES;

#endif

#if UNITY_IOS && !UNITY_EDITOR

            auth_to_use = FirebaseAuthType.APPLE_GAME_CENTER;

#endif
        }

        public void LogOut()
        {

#if USING_FIREBASE_AUTH && !UNITY_WEBGL

            if (auth != null)
            {
                if (user != null)
                {
                    auth.SignOut();

                    user = null;
                }
            }

#endif

        }

        public void LoginAsGuest(Action<FirebaseLoginObj> on_success, Action<FastErrorType> on_fail)
        {

#if USING_FIREBASE_AUTH && !UNITY_WEBGL

            LogOut();

            auth.SignInAnonymouslyAsync().ContinueWith(
               delegate (Task<Firebase.Auth.FirebaseUser> task)
               {
                   string error_msg = "";
                   Exception exception = null;

                   bool has_errors = task.HasErrors(out error_msg, out exception);

                   if (!has_errors)
                   {
                       Firebase.Auth.FirebaseUser curr_user = task.Result;

                       curr_user.TokenAsync(false).ContinueWith(token_task =>
                       {
                           has_errors = task.HasErrors(out error_msg, out exception);

                           if (!has_errors)
                           {
                               user = curr_user;

                               FirebaseLoginObj ret = new FirebaseLoginObj(FirebaseAuthType.AS_GUEST, "guest");

                               if (on_success != null)
                                   on_success.Invoke(ret);
                           }
                           else
                           {
                               Firebase.FirebaseException firebase_exception = GoogleFirebase.FirebaseExceptionToFastError.
                                   GetFirebaseExceptionFromException(exception);
                               FastErrorType error = GoogleFirebase.FirebaseExceptionToFastError.GetError(firebase_exception);

                               if (on_fail != null)
                                   on_fail.Invoke(error);
                           }

                       }, TaskScheduler.FromCurrentSynchronizationContext());
                   }
                   else
                   {
                       Firebase.FirebaseException firebase_exception = GoogleFirebase.FirebaseExceptionToFastError.
                           GetFirebaseExceptionFromException(exception);
                       FastErrorType error = GoogleFirebase.FirebaseExceptionToFastError.GetError(firebase_exception);

                       if (on_fail != null)
                           on_fail.Invoke(error);
                   }
               }
               , TaskScheduler.FromCurrentSynchronizationContext());

#endif

        }

        /// <summary>
        /// Requieres USING_FIREBASE_AUTH 
        /// </summary>
        public void LoginEmailPassword(string email, string password, Action<FirebaseLoginObj> on_success, Action<FastErrorType> on_fail)
        {

#if USING_FIREBASE_AUTH && !UNITY_WEBGL

            Firebase.Auth.Credential credential = Firebase.Auth.EmailAuthProvider.GetCredential(email, password);

            LoginWithCredentials(credential,
            delegate (string user_id)
            {
                FirebaseLoginObj ret = new FirebaseLoginObj(FirebaseAuthType.EMAIL_PASSWORD, user_id);

                if (on_success != null)
                    on_success.Invoke(ret);
            }
            , on_fail);

#else

            if (on_fail != null)
                on_fail.Invoke(FastErrorType.UNDEFINED);

#endif

        }

        /// <summary>
        /// Requieres USING_FIREBASE_AUTH and USING_GOOGLE_PLAY_SERVICES
        /// </summary>
        public void LoginGooglePlayGames(Action<FirebaseLoginObj> on_success, Action<string> on_google_play_fail,
            Action<FastErrorType> on_fail)
        {

#if UNITY_ANDROID && USING_FIREBASE_AUTH && USING_GOOGLE_PLAY_SERVICES

            try
            {
                UnityEngine.Social.Active.localUser.Authenticate(
                delegate(bool google_play_success, string google_play_error)
                {
                    if (google_play_success)
                    {
                        GooglePlayGames.PlayGamesLocalUser user = (GooglePlayGames.PlayGamesLocalUser)Social.Active.localUser;

                        Debug.Log("GetAnotherServerAuthCode");

                        GooglePlayGames.PlayGamesPlatform.Instance.GetAnotherServerAuthCode(true, 
                        delegate(string google_play_auth_code)
                        {
                            if (!string.IsNullOrEmpty(google_play_auth_code))
                            {
                                Debug.Log("Login google play auth code: " + google_play_auth_code);

                                Firebase.Auth.Credential credential = Firebase.Auth.PlayGamesAuthProvider.GetCredential(google_play_auth_code);

                                Debug.Log("Login in with credentials 1");

                                LoginWithCredentials(credential,
                                delegate (string user_id)
                                {
                                    FirebaseLoginObj ret = new FirebaseLoginObj(FirebaseAuthType.GOOGLE_PLAY_GAMES, user_id);

                                    if (on_success != null)
                                        on_success.Invoke(ret);
                                }
                                , on_fail);
                            }
                            else
                            {
                                if (on_fail != null)
                                    on_fail.Invoke(FastErrorType.GOOGLE_PLAY_EMPTY_AUTH_TOKEN);
                            }
                        });
                    }
                    else
                    {
                        if (on_google_play_fail != null)
                            on_google_play_fail.Invoke(google_play_error);
                    }
                });
            }
            catch(Exception exception)
            {
                if(exception != null)
                {
                    if (on_google_play_fail != null)
                        on_google_play_fail.Invoke("LoginGooglePlayGames exception:" + exception.StackTrace);

                    Debug.LogError(exception.StackTrace);
                }
            }

#else

            if (on_fail != null)
                on_fail.Invoke(FastErrorType.UNDEFINED);

#endif

        }

        /// <summary>
        /// Requieres USING_FIREBASE_AUTH
        /// </summary>
        public void LoginGameCenter(Action<FirebaseLoginObj> on_success, Action<string> on_game_center_fail,
            Action<FastErrorType> on_fail)
        {

#if UNITY_ANDROID && USING_FIREBASE_AUTH

            UnityEngine.Social.Active.localUser.Authenticate(delegate (bool game_center_success, string game_center_error)
            {
                if (game_center_success)
                {
                    Firebase.Auth.GameCenterAuthProvider.GetCredentialAsync().ContinueWith(
                    delegate (Task<Firebase.Auth.Credential> task)
                    {
                        string error_msg = "";
                        Exception exception = null;

                        bool has_errors = task.HasErrors(out error_msg, out exception);

                        if (!has_errors)
                        {
                            LoginWithCredentials(task.Result,
                            delegate (string user_id)
                            {
                                FirebaseLoginObj ret = new FirebaseLoginObj(FirebaseAuthType.APPLE_GAME_CENTER, user_id);

                                if (on_success != null)
                                    on_success.Invoke(ret);
                            }
                            , on_fail);
                        }
                        else
                        {
                            Firebase.FirebaseException firebase_exception = GoogleFirebase.FirebaseExceptionToFastError.
                                GetFirebaseExceptionFromException(exception);
                            FastErrorType error = GoogleFirebase.FirebaseExceptionToFastError.GetError(firebase_exception);

                            if (on_fail != null)
                                on_fail.Invoke(error);
                        }
                    });
                }
                else
                {
                    if (game_center_error != null)
                        on_game_center_fail.Invoke(game_center_error);
                }
            });

#else

            if (on_fail != null)
                on_fail.Invoke(FastErrorType.UNDEFINED);

#endif

        }

        public void LoginMobile(Action<FirebaseLoginObj> on_success, Action<string> on_service_fail,
            Action<FastErrorType> on_fail)
        {

#if UNITY_ANDROID && USING_FIREBASE_AUTH

            LoginGooglePlayGames(on_success, on_service_fail, on_fail);

#elif UNITY_IOS && USING_FIREBASE_AUTH

            LoginGameCenter(on_success, on_service_fail, on_fail)

#else

            if (on_fail != null)
                on_fail.Invoke(FastErrorType.UNDEFINED);

#endif

        }

#if USING_FIREBASE_AUTH && !UNITY_WEBGL

        /// <summary>
        /// Requieres USING_FIREBASE_AUTH
        /// </summary>
        private void LoginWithCredentials(Firebase.Auth.Credential credential, Action<string> on_success, Action<FastErrorType> on_fail)
        {
            LogOut();

            if (auth != null)
            {
                auth.SignInWithCredentialAsync(credential).ContinueWith(
                delegate (Task<Firebase.Auth.FirebaseUser> task)
                {
                    string error_msg = "";
                    Exception exception = null;

                    bool has_errors = task.HasErrors(out error_msg, out exception);

                    if (!has_errors)
                    {
                        Firebase.Auth.FirebaseUser curr_user = task.Result;

                        curr_user.TokenAsync(false).ContinueWith(token_task =>
                        {
                            has_errors = task.HasErrors(out error_msg, out exception);

                            if (!has_errors)
                            {
                                user = curr_user;

                                if (on_success != null)
                                    on_success.Invoke(token_task.Result);
                            }
                            else
                            {
                                Firebase.FirebaseException firebase_exception = GoogleFirebase.FirebaseExceptionToFastError.
                                    GetFirebaseExceptionFromException(exception);
                                FastErrorType error = GoogleFirebase.FirebaseExceptionToFastError.GetError(firebase_exception);

                                if (on_fail != null)
                                    on_fail.Invoke(error);
                            }

                        }, TaskScheduler.FromCurrentSynchronizationContext());
                    }
                    else
                    {
                        Firebase.FirebaseException firebase_exception = GoogleFirebase.FirebaseExceptionToFastError.
                            GetFirebaseExceptionFromException(exception);
                        FastErrorType error = GoogleFirebase.FirebaseExceptionToFastError.GetError(firebase_exception);

                        if (on_fail != null)
                            on_fail.Invoke(error);
                    }
                }
                , TaskScheduler.FromCurrentSynchronizationContext());
            }
            else
            {
                if (on_fail != null)
                    on_fail.Invoke(FastErrorType.FIREBASE_NOT_READY);
            }
        }

#endif

    /// <summary>
    /// Requieres USING_FIREBASE_AUTH
    /// </summary>
    public void RegisterEmailPassword(string username, string email, string password, string password_conf,
            Action on_success, Action<FastErrorType> on_fail)
        {

#if USING_FIREBASE_AUTH && !UNITY_WEBGL

            FastErrorType error_type = FastErrorType.UNDEFINED;

            bool registration_error = Authentication.AuthenticationDataToFastError.GetRegistrationError(username, email, password, 
                password_conf, out error_type);

            if (!registration_error)
            {
                if (auth != null)
                {
                    auth.CreateUserWithEmailAndPasswordAsync(email, password).ContinueWith(
                    delegate (Task task)
                    {
                        string error_msg = "";
                        Exception exception = null;

                        bool has_errors = task.HasErrors(out error_msg, out exception);

                        if (!has_errors)
                        {
                            if (on_success != null)
                                on_success.Invoke();
                        }
                        else
                        {
                            Firebase.FirebaseException firebase_exception = GoogleFirebase.FirebaseExceptionToFastError.
                                GetFirebaseExceptionFromException(exception);
                            FastErrorType error = GoogleFirebase.FirebaseExceptionToFastError.GetError(firebase_exception);

                            if (on_fail != null)
                                on_fail.Invoke(error);
                        }

                    }, TaskScheduler.FromCurrentSynchronizationContext());
                }
                else
                {
                    if (on_fail != null)
                        on_fail.Invoke(FastErrorType.FIREBASE_NOT_READY);
                }
            }
            else
            {
                if (on_fail != null)
                    on_fail.Invoke(error_type);
            }
#else

            if (on_fail != null)
                on_fail.Invoke(FastErrorType.UNDEFINED);

#endif

        }

        /// <summary>
        /// Requieres USING_FIREBASE_AUTH
        /// </summary>
        public void UpdateDisplayName(string display_name, Action on_success, Action<FastErrorType> on_fail)
        {

#if USING_FIREBASE_AUTH && !UNITY_WEBGL

            Firebase.Auth.UserProfile profile = new Firebase.Auth.UserProfile
            {
                DisplayName = display_name,
            };

            user.UpdateUserProfileAsync(profile).ContinueWith(
            delegate (Task update_task)
            {
                string error_msg = "";
                Exception exception = null;

                bool has_errors = update_task.HasErrors(out error_msg, out exception);

                if (!has_errors)
                {
                    if (on_success != null)
                        on_success.Invoke();
                }
                else
                {
                    Firebase.FirebaseException firebase_exception = GoogleFirebase.FirebaseExceptionToFastError.
                        GetFirebaseExceptionFromException(exception);
                    FastErrorType error = GoogleFirebase.FirebaseExceptionToFastError.GetError(firebase_exception);

                    if (on_fail != null)
                        on_fail.Invoke(error);
                }

            }, TaskScheduler.FromCurrentSynchronizationContext());

#else

            if (on_fail != null)
                on_fail.Invoke(FastErrorType.UNDEFINED);

#endif

        }

        /// <summary>
        /// Name of the event to log.
        /// Parameter type supplies information that contextualize analytics events.
        /// Requieres USING_FIREBASE_ANALYTICS
        /// </summary>
        public void SendAnalytics(string name, string parameter_type, string parameter_value)
        {

#if USING_FIREBASE_ANALYTICS && !UNITY_WEBGL

            Firebase.Analytics.FirebaseAnalytics.LogEvent(name, parameter_type, parameter_value);

#endif

        }

        /// <summary>
        /// Name of the event to log.
        /// Parameter type supplies information that contextualize analytics events.
        /// Requieres USING_FIREBASE_ANALYTICS
        /// </summary>
        public void SendAnalytics(string name, string parameter_type, int parameter_value)
        {

#if USING_FIREBASE_ANALYTICS && !UNITY_WEBGL

            Firebase.Analytics.FirebaseAnalytics.LogEvent(name, parameter_type, parameter_value);

#endif

        }

        /// <summary>
        /// Name of the event to log.
        /// Parameter type supplies information that contextualize analytics events.
        /// </summary>
        public void SendAnalytics(string name, string parameter_type, float parameter_value)
        {

#if USING_FIREBASE_ANALYTICS && !UNITY_WEBGL

            Firebase.Analytics.FirebaseAnalytics.LogEvent(name, parameter_type, parameter_value);

#endif

        }
    }
}

