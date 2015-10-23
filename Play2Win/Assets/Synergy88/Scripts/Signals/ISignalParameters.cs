using System;

public interface ISignalParameters {

	/**
	 * Adds a parameter
	 */
	void AddParameter(string key, object value);
	
	/**
	 * Gets a parameter value
	 * This used to be a template method but will not work on iOS (JIT-AOT thing)
	 */
	object GetParameter(string key);

	/**
	 * Returns whether or not it has a parameter with the specified key
	 */
	bool HasParameter(string key);

	/**
	 * Clears all parameters
	 */
	void Clear();

}
