using UnityEngine;
using GameJolt.External.SimpleJSON;

namespace GameJolt.API.Core {
	/// <summary>
	/// API Response Formats.
	/// </summary>
	public enum ResponseFormat {
		Dump,
		Json,
		Raw,
		Texture
	}

	/// <summary>
	/// Response object to parse API responses.
	/// </summary>
	public class Response {
		/// <summary>
		/// The Response Format.
		/// </summary>
		public readonly ResponseFormat Format;

		/// <summary>
		/// Whether the response is successful.
		/// </summary>
		public readonly bool Success;

		/// <summary>
		/// The response bytes.
		/// </summary>
		/// <remarks>
		/// <para>
		/// Only populated when the <see cref="ResponseFormat"/> is `Raw`. 
		/// </para>
		/// </remarks>
		public readonly byte[] Bytes;

		/// <summary>
		/// The response dump.
		/// </summary>
		/// <remarks>
		/// <para>
		/// Only populated when the <see cref="ResponseFormat"/>  is `Dump`.
		/// </para>
		/// </remarks>
		public readonly string Dump;

		/// <summary>
		/// The response JSON.
		/// </summary>
		/// <para>
		/// Only populated when the <see cref="ResponseFormat"/>  is `Json`.
		/// </para>
		public readonly JSONNode Json;

		/// <summary>
		/// The response texture.
		/// </summary>
		/// <remarks>
		/// <para>
		/// Only populated when the <see cref="ResponseFormat"/> is `Texture`. 
		/// </para>
		/// </remarks>
		public readonly Texture2D Texture;

		/// <summary>
		/// Initializes a new instance of the <see cref="Response"/> class.
		/// </summary>
		/// <param name="errorMessage">Error message.</param>
		public Response(string errorMessage) {
			Success = false;
			LogHelper.Warning(errorMessage);
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Response"/> class.
		/// </summary>
		/// <param name="www">The API Fesponse.</param>
		/// <param name="format">The format of the response.</param>
		public Response(WWW www, ResponseFormat format = ResponseFormat.Json) {
			if(!string.IsNullOrEmpty(www.error)) {
				Success = false;
				LogHelper.Warning(www.error);
				return;
			}

			Format = format;

			switch(format) {
				case ResponseFormat.Dump:
					Success = www.text.StartsWith("SUCCESS");
					var returnIndex = www.text.IndexOf('\n');
					if(returnIndex != -1) {
						Dump = www.text.Substring(returnIndex + 1);
					}

					if(!Success) {
						LogHelper.Warning(Dump);
						Dump = null;
					}
					break;
				case ResponseFormat.Json:
					Json = JSON.Parse(www.text)["response"];
					Success = Json["success"].AsBool;
					if(!Success) {
						LogHelper.Warning(Json["message"]);
						Json = null;
					}
					break;
				case ResponseFormat.Raw:
					Success = true;
					Bytes = www.bytes;
					break;
				case ResponseFormat.Texture:
					Success = true;
					Texture = www.texture;
					break;
				default:
					Success = false;
					LogHelper.Warning("Unknown format. Cannot process response.");
					break;
			}
		}
	}
}
