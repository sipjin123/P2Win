using UnityEngine;
using System.Collections;

/**
 * Contains vector related utility methods.
 */
public static class VectorUtils {
	
	// a constant vector where the components are all 0
	public static readonly Vector3 ZERO = new Vector3(0, 0, 0);
	
	// a constant vector where the components are all 1
	public static readonly Vector3 ONE = new Vector3(1, 1, 1);
	
	/**
	 * Returns whether or not the specified vectors a and b are equal.
	 */
	public static bool Equals(Vector3 a, Vector3 b) {
		return Comparison.TolerantEquals(a.x, b.x) &&
			Comparison.TolerantEquals(a.y, b.y) &&
			Comparison.TolerantEquals(a.z, b.z);
	}
	
}

