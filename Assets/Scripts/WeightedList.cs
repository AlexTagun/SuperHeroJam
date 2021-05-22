using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeightedList<T> where T : Object {
    private class Entry {
        public float AccumulatedWeight;
        public T Item;
    }

    private List<Entry> _entries = new List<Entry>();
    private float _accumulatedWeight;
    private Random _rand = new Random();

    public void AddEntry(T item, float weight) {
        _accumulatedWeight += weight;
        Entry e = new Entry();
        e.Item = item;
        e.AccumulatedWeight = _accumulatedWeight;
        _entries.Add(e);
    }

    public T GetRandom() {
        float r = Random.value * _accumulatedWeight;

        foreach (Entry entry in _entries) {
            if (entry.AccumulatedWeight >= r) {
                return entry.Item;
            }
        }

        return null; //should only happen when there are no entries
    }
}