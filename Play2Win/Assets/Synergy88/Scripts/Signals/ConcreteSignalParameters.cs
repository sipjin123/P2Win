using System;
using System.Collections.Generic;

class ConcreteSignalParameters : ISignalParameters {

	private Dictionary<string, object> parameterMap;

	/**
	 * Constructor
	 */
	public ConcreteSignalParameters() {
		this.parameterMap = new Dictionary<string, object>();
	}

	#region ISignalParameters implementation
	public void AddParameter(string key, object value) {
		this.parameterMap[key] = value;
	}
	
	public object GetParameter(string key) {
		return this.parameterMap[key];
	}

	public bool HasParameter(string key) {
		return this.parameterMap.ContainsKey(key);
	}

	public void Clear() {
		this.parameterMap.Clear();
	}
	#endregion

}