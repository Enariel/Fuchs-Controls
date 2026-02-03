#region Meta
// FuchsControls
// Created: 03/02/2026
// Modified: 03/02/2026
#endregion

namespace FuchsControls.Extensions;

public static class ElementExtensions
{
	 /// <summary>
    /// Find the first element of type TElement in the visual tree.
    /// </summary>
    /// <param name="element">The root element to search.</param>
    /// <typeparam name="TElement">The target element.</typeparam>
    /// <returns><see cref="TElement"/> where TElement is <see cref="View"/>.</returns>
    public static TElement? FindInVisualTree<TElement>(this Element? element) where TElement : View
    {
        if (element is TElement target)
            return target;

        if (element is Layout layout)
        {
            foreach (var child in layout.Children)
            {
                var result = FindInVisualTree<TElement>(child as Element);
                if (result != null)
                    return result;
            }
        }
        else if (element is ContentView contentView && contentView.Content != null)
            return FindInVisualTree<TElement>(contentView.Content);
        else if (element is Border border && border.Content != null)
            return FindInVisualTree<TElement>(border.Content);

        return null;
    }

    /// <summary>
    /// Finds all elements of type TElement in the visual tree.
    /// </summary>
    /// <param name="element">The root element to search.</param>
    /// <typeparam name="TElement">The target element type to find.</typeparam>
    /// <returns>An enumeration of elements of type TElement found in the visual tree.</returns>
    public static IEnumerable<TElement?> FindElementsInVisualTree<TElement>(this Element? element) where TElement : View
    {
        if (element is TElement target)
            yield return target;

        if (element is Layout layout)
        {
            foreach (var child in layout.Children)
            {
                foreach (var result in FindElementsInVisualTree<TElement>(child as Element))
                    yield return result;
            }
        }
        else if (element is ContentView contentView && contentView.Content != null)
            foreach (var result in FindElementsInVisualTree<TElement>(contentView.Content))
                yield return result;
        else if (element is Border border && border.Content != null)
            foreach (var result in FindElementsInVisualTree<TElement>(border.Content))
                yield return result;
    }

    /// <summary>
    /// Finds all elements of the specified types in the visual tree.
    /// </summary>
    /// <param name="element">The root element to search.</param>
    /// <param name="types">The target element types to find.</param>
    /// <returns>An enumeration of elements of the specified types found in the visual tree.</returns>
    public static IEnumerable<View> FindElementsInVisualTree(this Element? element, params Type[] types)
    {
        if (element is View view && types.Any(t => t.IsAssignableFrom(element.GetType())))
            yield return view;

        if (element is Layout layout)
        {
            foreach (var child in layout.Children)
            {
                foreach (var result in FindElementsInVisualTree(child as Element, types))
                    yield return result;
            }
        }
        else if (element is ContentView contentView && contentView.Content != null)
            foreach (var result in FindElementsInVisualTree(contentView.Content, types))
                yield return result;
        else if (element is Border border && border.Content != null)
            foreach (var result in FindElementsInVisualTree(border.Content, types))
                yield return result;
    }

    /// <summary>
    /// Finds all elements of type T1 or T2 in the visual tree.
    /// </summary>
    /// <param name="element">The root element to search.</param>
    /// <typeparam name="T1">The first target element type to find.</typeparam>
    /// <typeparam name="T2">The second target element type to find.</typeparam>
    /// <returns>An enumeration of elements of type T1 or T2 found in the visual tree.</returns>
    public static IEnumerable<View> FindElementsInVisualTree<T1, T2>(this Element? element)
        where T1 : View
        where T2 : View
    {
        if (element is T1 target1)
            yield return target1;
        else if (element is T2 target2)
            yield return target2;

        if (element is Layout layout)
        {
            foreach (var child in layout.Children)
            {
                foreach (var result in FindElementsInVisualTree<T1, T2>(child as Element))
                    yield return result;
            }
        }
        else if (element is ContentView contentView && contentView.Content != null)
            foreach (var result in FindElementsInVisualTree<T1, T2>(contentView.Content))
                yield return result;
        else if (element is Border border && border.Content != null)
            foreach (var result in FindElementsInVisualTree<T1, T2>(border.Content))
                yield return result;
    }

    /// <summary>
    /// Finds all elements of type T1, T2, or T3 in the visual tree.
    /// </summary>
    /// <param name="element">The root element to search.</param>
    /// <typeparam name="T1">The first target element type to find.</typeparam>
    /// <typeparam name="T2">The second target element type to find.</typeparam>
    /// <typeparam name="T3">The third target element type to find.</typeparam>
    /// <returns>An enumeration of elements of type T1, T2, or T3 found in the visual tree.</returns>
    public static IEnumerable<View> FindElementsInVisualTree<T1, T2, T3>(this Element? element)
        where T1 : View
        where T2 : View
        where T3 : View
    {
        if (element is T1 target1)
            yield return target1;
        else if (element is T2 target2)
            yield return target2;
        else if (element is T3 target3)
            yield return target3;

        if (element is Layout layout)
        {
            foreach (var child in layout.Children)
            {
                foreach (var result in FindElementsInVisualTree<T1, T2, T3>(child as Element))
                    yield return result;
            }
        }
        else if (element is ContentView contentView && contentView.Content != null)
            foreach (var result in FindElementsInVisualTree<T1, T2, T3>(contentView.Content))
                yield return result;
        else if (element is Border border && border.Content != null)
            foreach (var result in FindElementsInVisualTree<T1, T2, T3>(border.Content))
                yield return result;
    }
    /// <summary>
    /// Finds all elements of type T1, T2, T3, or T4 in the visual tree.
    /// </summary>
    /// <param name="element">The root element to search.</param>
    /// <typeparam name="T1">The first target element type to find.</typeparam>
    /// <typeparam name="T2">The second target element type to find.</typeparam>
    /// <typeparam name="T3">The third target element type to find.</typeparam>
    /// <typeparam name="T4">The fourth target element type to find.</typeparam>
    /// <returns>An enumeration of elements of type T1, T2, T3, or T4 found in the visual tree.</returns>
    public static IEnumerable<View> FindElementsInVisualTree<T1, T2, T3, T4>(this Element? element)
        where T1 : View
        where T2 : View
        where T3 : View
        where T4 : View
    {
        if (element is T1 target1)
            yield return target1;
        else if (element is T2 target2)
            yield return target2;
        else if (element is T3 target3)
            yield return target3;
        else if (element is T4 target4)
            yield return target4;

        if (element is Layout layout)
        {
            foreach (var child in layout.Children)
            {
                foreach (var result in FindElementsInVisualTree<T1, T2, T3, T4>(child as Element))
                    yield return result;
            }
        }
        else if (element is ContentView contentView && contentView.Content != null)
            foreach (var result in FindElementsInVisualTree<T1, T2, T3, T4>(contentView.Content))
                yield return result;
        else if (element is Border border && border.Content != null)
            foreach (var result in FindElementsInVisualTree<T1, T2, T3, T4>(border.Content))
                yield return result;
    }

    /// <summary>
    /// Finds all elements of type T1, T2, T3, T4, or T5 in the visual tree.
    /// </summary>
    /// <param name="element">The root element to search.</param>
    /// <typeparam name="T1">The first target element type to find.</typeparam>
    /// <typeparam name="T2">The second target element type to find.</typeparam>
    /// <typeparam name="T3">The third target element type to find.</typeparam>
    /// <typeparam name="T4">The fourth target element type to find.</typeparam>
    /// <typeparam name="T5">The fifth target element type to find.</typeparam>
    /// <returns>An enumeration of elements of type T1, T2, T3, T4, or T5 found in the visual tree.</returns>
    public static IEnumerable<View> FindElementsInVisualTree<T1, T2, T3, T4, T5>(this Element? element)
        where T1 : View
        where T2 : View
        where T3 : View
        where T4 : View
        where T5 : View
    {
        if (element is T1 target1)
            yield return target1;
        else if (element is T2 target2)
            yield return target2;
        else if (element is T3 target3)
            yield return target3;
        else if (element is T4 target4)
            yield return target4;
        else if (element is T5 target5)
            yield return target5;

        if (element is Layout layout)
        {
            foreach (var child in layout.Children)
            {
                foreach (var result in FindElementsInVisualTree<T1, T2, T3, T4, T5>(child as Element))
                    yield return result;
            }
        }
        else if (element is ContentView contentView && contentView.Content != null)
            foreach (var result in FindElementsInVisualTree<T1, T2, T3, T4, T5>(contentView.Content))
                yield return result;
        else if (element is Border border && border.Content != null)
            foreach (var result in FindElementsInVisualTree<T1, T2, T3, T4, T5>(border.Content))
                yield return result;
    }

    /// <summary>
    /// Finds all elements of type T1, T2, T3, T4, T5, or T6 in the visual tree.
    /// </summary>
    /// <param name="element">The root element to search.</param>
    /// <typeparam name="T1">The first target element type to find.</typeparam>
    /// <typeparam name="T2">The second target element type to find.</typeparam>
    /// <typeparam name="T3">The third target element type to find.</typeparam>
    /// <typeparam name="T4">The fourth target element type to find.</typeparam>
    /// <typeparam name="T5">The fifth target element type to find.</typeparam>
    /// <typeparam name="T6">The sixth target element type to find.</typeparam>
    /// <returns>An enumeration of elements of type T1, T2, T3, T4, T5, or T6 found in the visual tree.</returns>
    public static IEnumerable<View> FindElementsInVisualTree<T1, T2, T3, T4, T5, T6>(this Element? element)
        where T1 : View
        where T2 : View
        where T3 : View
        where T4 : View
        where T5 : View
        where T6 : View
    {
        if (element is T1 target1)
            yield return target1;
        else if (element is T2 target2)
            yield return target2;
        else if (element is T3 target3)
            yield return target3;
        else if (element is T4 target4)
            yield return target4;
        else if (element is T5 target5)
            yield return target5;
        else if (element is T6 target6)
            yield return target6;

        if (element is Layout layout)
        {
            foreach (var child in layout.Children)
            {
                foreach (var result in FindElementsInVisualTree<T1, T2, T3, T4, T5, T6>(child as Element))
                    yield return result;
            }
        }
        else if (element is ContentView contentView && contentView.Content != null)
            foreach (var result in FindElementsInVisualTree<T1, T2, T3, T4, T5, T6>(contentView.Content))
                yield return result;
        else if (element is Border border && border.Content != null)
            foreach (var result in FindElementsInVisualTree<T1, T2, T3, T4, T5, T6>(border.Content))
                yield return result;
    }
    /// <summary>
    /// Finds all elements of the specified types in the visual tree and returns them with their type information.
    /// </summary>
    /// <param name="element">The root element to search.</param>
    /// <param name="types">The target element types to find.</param>
    /// <returns>An enumeration of tuples containing the found elements and their types.</returns>
    public static IEnumerable<(View Element, Type Type)> FindElementsInVisualTreeWithType(this Element? element, params Type[] types)
    {
        if (element is View view)
        {
            var matchingType = types.FirstOrDefault(t => t.IsAssignableFrom(element.GetType()));
            if (matchingType != null)
                yield return (view, matchingType);
        }

        if (element is Layout layout)
        {
            foreach (var child in layout.Children)
            {
                foreach (var result in FindElementsInVisualTreeWithType(child as Element, types))
                    yield return result;
            }
        }
        else if (element is ContentView contentView && contentView.Content != null)
            foreach (var result in FindElementsInVisualTreeWithType(contentView.Content, types))
                yield return result;
        else if (element is Border border && border.Content != null)
            foreach (var result in FindElementsInVisualTreeWithType(border.Content, types))
                yield return result;
    }
}