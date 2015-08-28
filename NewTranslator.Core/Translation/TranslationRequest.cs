using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace NewTranslator.Core.Translation
{
	public class TranslationRequest
	{
		public string Text { get; set; }
		public IEnumerable<BaseTranslator> Translators { get; set; }
		public string SourceLanguage { get; set; }
		public string TargetLanguage { get; set; }
		public IEnumerable<TranslationResult> Results { get; set; }
		public IEnumerable<Exception> Exceptions { get; set; }

		public TranslationRequest(string text, string sourceLanguage, string targetLanguage, params BaseTranslator[] translators)
		{
		    if (translators == null || !translators.Any()) throw new ArgumentNullException(nameof(translators));

		    Text = text;
			Translators = translators;
			SourceLanguage = sourceLanguage;
			TargetLanguage = targetLanguage;
		}

		public void GetTranslationAsync()
		{
			new Thread(TranslationThread).Start();			
		}

        private async void TranslationThread()
        {
            Results = null;
            Exceptions = null;

            var results = new List<TranslationResult>();
            var errors = new List<Exception>();

            var splitIdentifiers = SplitIdentifiers(Text);
            var tasksList = new List<Task<TranslationResult>>();
            foreach (var translator in Translators)
            {
                var task = translator.GetTranslationAsync(splitIdentifiers, SourceLanguage, TargetLanguage);
                var errorHandlingTask = task.ContinueWith((t) => errors.Add(t.Exception), TaskContinuationOptions.OnlyOnFaulted);
                var successHandlingTask = task.ContinueWith((t) => results.Add(t.Result), TaskContinuationOptions.OnlyOnRanToCompletion);
                tasksList.Add(task);
            }
            
            try
            {
                await Task.WhenAll(tasksList);
            }
            catch (Exception e)
            {
                errors.Insert(0, e);
                Exceptions = errors;
            }

            Results = results;

            OnTranslationComplete();
        }
        
	    public event EventHandler TranslationComplete;
		private void OnTranslationComplete()
		{
			if (TranslationComplete != null)
				TranslationComplete(this, EventArgs.Empty);
		}

		private static readonly Regex _camelCaseRegex = new Regex(@"\p{Ll}\p{Lu}", RegexOptions.Compiled | RegexOptions.Multiline);

		/// <summary>
		/// splits prefix_camelCase -> prefix camel case
		/// </summary>		
		private static string SplitIdentifiers(string text)
		{
			return _camelCaseRegex.Replace(text.Replace("_", " "), m => m.Value[0] + " " + m.Value[1].ToString().ToLower());
		}

	}
}
