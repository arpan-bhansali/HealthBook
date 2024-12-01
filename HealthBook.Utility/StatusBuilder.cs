using static HealthBook.Utility.EnumList;

namespace HealthBook.Utility
{
    public static class StatusBuilder
    {
        /// <summary>
        /// Returns Success with data
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static BaseResponseModel ResponseSuccessStatus(object data)
        {
            return new BaseResponseModel() { StatusCode = Convert.ToInt64(ResponseType.Success), Data = data };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public static BaseResponseModel ResponseSuccessStatus(object data, string message)
        {
            return new BaseResponseModel() { StatusCode = Convert.ToInt64(ResponseType.Success), Data = data, Message = message };
        }

        /// <summary>
        /// Returns Success message
        /// </summary>
        /// <param name="messageType"></param>
        /// <returns></returns>
        public static BaseResponseModel ResponseSuccessStatus(string message)
        {
            return new BaseResponseModel() { StatusCode = Convert.ToInt64(ResponseType.Success), Message = message };
        }


        /// <summary>
        /// Returns Fail message with invalid message
        /// </summary>
        /// <param name="messageType"></param>
        /// <returns></returns>
        public static BaseResponseModel ResponseFailStatus(string message)
        {
            return new BaseResponseModel() { StatusCode = Convert.ToInt64(ResponseType.Error), Message = message };
        }

        /// <summary>
        /// Returns Fail message with invalid message and null data
        /// </summary>
        /// <param name="data"></param>
        /// <param name="messageType"></param>
        /// <returns></returns>
        public static BaseResponseModel ResponseFailStatus(object data, string message)
        {
            return new BaseResponseModel() { StatusCode = Convert.ToInt64(ResponseType.Error), Data = data, Message = message };
        }


        /// <summary>
        /// Returns Exception message
        /// </summary>
        /// <param name="messageType"></param>
        /// <returns></returns>
        public static BaseResponseModel ResponseExceptionStatus(Exception ex)
        {
            return new BaseResponseModel() { StatusCode = Convert.ToInt64(ResponseType.Exception), Message = ex.Message };
        }
    }
}
