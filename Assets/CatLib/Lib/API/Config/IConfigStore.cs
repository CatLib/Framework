/*
 * This file is part of the CatLib package.
 *
 * (c) Yu Bin <support@catlib.io>
 *
 * For the full copyright and license information, please view the LICENSE
 * file that was distributed with this source code.
 *
 * Document: http://catlib.io/
 */
 
using System;

namespace CatLib.API.Config{

	public interface IConfigStore{

		T Get<T>(string name, string field , T def = default(T));

		T Get<T>(Type name , string field , T def = default(T));

		object Get(string name , string field , object def);

		object Get(Type name , string field , object def);

		string Get(string name , string field , string def);

		string Get(Type name , string field , string def);

	}

}