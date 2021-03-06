﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace DiamondFlasher.Helpers
{
	public interface IReadonlySet<T> : IReadOnlyCollection<T>, IEnumerable<T>
	{
		bool Contains(T item);
	}
	public class ReadOnlySet<T> : ICollection<T>, IEnumerable<T>, IEnumerable, IReadOnlyCollection<T>, ICollection, ISet<T>, IReadonlySet<T>
	{
		protected ISet<T> collection;
		public ReadOnlySet(ISet<T> collection)
		{
			this.collection = collection ?? throw new ArgumentNullException(nameof(collection));
		}

		public int Count => collection.Count;

		public bool IsSynchronized => false;

		public object SyncRoot => null;
		public bool Contains(T element)
		{
			return collection.Contains(element);
		}
		int ICollection<T>.Count => Count;
		bool ICollection<T>.IsReadOnly => true;
		public IEnumerator<T> GetEnumerator()
		{
			foreach (var item in collection)
				yield return item;
		}
		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}


		void ICollection<T>.Add(T item)
		{
			throw new NotSupportedException();
		}

		void ICollection<T>.Clear()
		{
			throw new NotSupportedException();
		}

		bool ICollection<T>.Contains(T item)
		{
			return collection.Contains(item);
		}

		void ICollection<T>.CopyTo(T[] array, int arrayIndex)
		{
			collection.CopyTo(array, arrayIndex);
		}

		bool ICollection<T>.Remove(T item)
		{
			throw new NotSupportedException();
		}

		public void CopyTo(Array array, int index)
		{
			(collection as ICollection).CopyTo(array, index);
		}

		bool ISet<T>.Add(T item)
		{
			throw new NotSupportedException();
		}

		void ISet<T>.ExceptWith(IEnumerable<T> other) => throw new NotImplementedException();

		void ISet<T>.IntersectWith(IEnumerable<T> other) => throw new NotImplementedException();

		bool ISet<T>.IsProperSubsetOf(IEnumerable<T> other)
		{
			return collection.IsProperSubsetOf(other);
		}

		bool ISet<T>.IsProperSupersetOf(IEnumerable<T> other)
		{
			return collection.IsProperSupersetOf(other);
		}

		bool ISet<T>.IsSubsetOf(IEnumerable<T> other)
		{
		
			return collection.IsSubsetOf(other);
		}

		bool ISet<T>.IsSupersetOf(IEnumerable<T> other)
		{
			return collection.IsSupersetOf(other);
		}

		bool ISet<T>.Overlaps(IEnumerable<T> other)
		{
			return collection.Overlaps(other);
		}

		bool ISet<T>.SetEquals(IEnumerable<T> other)
		{
			
			return collection.SetEquals(other);
			
		}

		void ISet<T>.SymmetricExceptWith(IEnumerable<T> other) => throw new NotImplementedException();

		void ISet<T>.UnionWith(IEnumerable<T> other) => throw new NotImplementedException();

	}

}
