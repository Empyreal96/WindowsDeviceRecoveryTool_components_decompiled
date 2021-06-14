using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Security;
using System.Windows.Media;
using System.Windows.Media.Animation;
using MS.Internal;

namespace System.Windows
{
	/// <summary>Manages states and the logic for transitioning between states for controls.</summary>
	// Token: 0x02000139 RID: 313
	public class VisualStateManager : DependencyObject
	{
		// Token: 0x06000CF1 RID: 3313 RVA: 0x0002FF14 File Offset: 0x0002E114
		private static bool GoToStateCommon(FrameworkElement control, FrameworkElement stateGroupsRoot, string stateName, bool useTransitions)
		{
			if (stateName == null)
			{
				throw new ArgumentNullException("stateName");
			}
			if (stateGroupsRoot == null)
			{
				return false;
			}
			IList<VisualStateGroup> visualStateGroupsInternal = VisualStateManager.GetVisualStateGroupsInternal(stateGroupsRoot);
			if (visualStateGroupsInternal == null)
			{
				return false;
			}
			VisualStateGroup group;
			VisualState visualState;
			VisualStateManager.TryGetState(visualStateGroupsInternal, stateName, out group, out visualState);
			VisualStateManager customVisualStateManager = VisualStateManager.GetCustomVisualStateManager(stateGroupsRoot);
			if (customVisualStateManager != null)
			{
				return customVisualStateManager.GoToStateCore(control, stateGroupsRoot, stateName, group, visualState, useTransitions);
			}
			return visualState != null && VisualStateManager.GoToStateInternal(control, stateGroupsRoot, group, visualState, useTransitions);
		}

		/// <summary>Transitions the control between two states. Use this method to transition states on control that has a <see cref="T:System.Windows.Controls.ControlTemplate" />.</summary>
		/// <param name="control">The control to transition between states. </param>
		/// <param name="stateName">The state to transition to.</param>
		/// <param name="useTransitions">
		///       <see langword="true" /> to use a <see cref="T:System.Windows.VisualTransition" /> object to transition between states; otherwise, <see langword="false" />.</param>
		/// <returns>
		///     <see langword="true" /> if the control successfully transitioned to the new state; otherwise, <see langword="false" />.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="control" /> is <see langword="null" />.-or-
		///         <paramref name="stateName" /> is <see langword="null" />.</exception>
		// Token: 0x06000CF2 RID: 3314 RVA: 0x0002FF74 File Offset: 0x0002E174
		public static bool GoToState(FrameworkElement control, string stateName, bool useTransitions)
		{
			if (control == null)
			{
				throw new ArgumentNullException("control");
			}
			FrameworkElement stateGroupsRoot = control.StateGroupsRoot;
			return VisualStateManager.GoToStateCommon(control, stateGroupsRoot, stateName, useTransitions);
		}

		/// <summary>Transitions the element between two states. Use this method to transition states that are defined by an application, rather than defined by a control.</summary>
		/// <param name="stateGroupsRoot">The root element that contains the <see cref="T:System.Windows.VisualStateManager" />.</param>
		/// <param name="stateName">The state to transition to.</param>
		/// <param name="useTransitions">
		///       <see langword="true" /> to use a <see cref="T:System.Windows.VisualTransition" /> object to transition between states; otherwise, <see langword="false" />.</param>
		/// <returns>
		///     <see langword="true" /> if the control successfully transitioned to the new state; otherwise, <see langword="false" />.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="stateGroupsRoot" /> is <see langword="null" />.-or-
		///         <paramref name="stateName" /> is <see langword="null" />.</exception>
		// Token: 0x06000CF3 RID: 3315 RVA: 0x0002FF9F File Offset: 0x0002E19F
		public static bool GoToElementState(FrameworkElement stateGroupsRoot, string stateName, bool useTransitions)
		{
			if (stateGroupsRoot == null)
			{
				throw new ArgumentNullException("stateGroupsRoot");
			}
			return VisualStateManager.GoToStateCommon(null, stateGroupsRoot, stateName, useTransitions);
		}

		/// <summary>Transitions a control between states.</summary>
		/// <param name="control">The control to transition between states. </param>
		/// <param name="stateGroupsRoot">The root element that contains the <see cref="T:System.Windows.VisualStateManager" />.</param>
		/// <param name="stateName">The name of the state to transition to.</param>
		/// <param name="group">The <see cref="T:System.Windows.VisualStateGroup" /> that the state belongs to.</param>
		/// <param name="state">The representation of the state to transition to.</param>
		/// <param name="useTransitions">
		///       <see langword="true" /> to use a <see cref="T:System.Windows.VisualTransition" /> object to transition between states; otherwise, <see langword="false" />.</param>
		/// <returns>
		///     <see langword="true" /> if the control successfully transitioned to the new state; otherwise, <see langword="false" />.</returns>
		// Token: 0x06000CF4 RID: 3316 RVA: 0x0002FFB8 File Offset: 0x0002E1B8
		protected virtual bool GoToStateCore(FrameworkElement control, FrameworkElement stateGroupsRoot, string stateName, VisualStateGroup group, VisualState state, bool useTransitions)
		{
			return VisualStateManager.GoToStateInternal(control, stateGroupsRoot, group, state, useTransitions);
		}

		/// <summary>Gets the <see cref="P:System.Windows.VisualStateManager.CustomVisualStateManager" /> attached property.</summary>
		/// <param name="obj">The element to get the <see cref="P:System.Windows.VisualStateManager.CustomVisualStateManager" /> attached property from.</param>
		/// <returns>The visual state manager that transitions between the states of a control. </returns>
		// Token: 0x06000CF5 RID: 3317 RVA: 0x0002FFC7 File Offset: 0x0002E1C7
		public static VisualStateManager GetCustomVisualStateManager(FrameworkElement obj)
		{
			if (obj == null)
			{
				throw new ArgumentNullException("obj");
			}
			return obj.GetValue(VisualStateManager.CustomVisualStateManagerProperty) as VisualStateManager;
		}

		/// <summary>Sets the <see cref="P:System.Windows.VisualStateManager.CustomVisualStateManager" /> attached property.</summary>
		/// <param name="obj">The object to set the property on.</param>
		/// <param name="value">The visual state manager that transitions between the states of a control.</param>
		// Token: 0x06000CF6 RID: 3318 RVA: 0x0002FFE7 File Offset: 0x0002E1E7
		public static void SetCustomVisualStateManager(FrameworkElement obj, VisualStateManager value)
		{
			if (obj == null)
			{
				throw new ArgumentNullException("obj");
			}
			obj.SetValue(VisualStateManager.CustomVisualStateManagerProperty, value);
		}

		// Token: 0x06000CF7 RID: 3319 RVA: 0x00030004 File Offset: 0x0002E204
		internal static Collection<VisualStateGroup> GetVisualStateGroupsInternal(FrameworkElement obj)
		{
			if (obj == null)
			{
				throw new ArgumentNullException("obj");
			}
			bool flag;
			BaseValueSourceInternal valueSource = obj.GetValueSource(VisualStateManager.VisualStateGroupsProperty, null, out flag);
			if (valueSource != BaseValueSourceInternal.Default)
			{
				return obj.GetValue(VisualStateManager.VisualStateGroupsProperty) as Collection<VisualStateGroup>;
			}
			return null;
		}

		/// <summary>Gets the <see cref="P:System.Windows.VisualStateManager.VisualStateGroups" /> attached property.</summary>
		/// <param name="obj">The element to get the <see cref="P:System.Windows.VisualStateManager.VisualStateGroups" /> attached property from.</param>
		/// <returns>The collection of <see cref="T:System.Windows.VisualStateGroup" /> objects that is associated with the specified object.</returns>
		// Token: 0x06000CF8 RID: 3320 RVA: 0x00030044 File Offset: 0x0002E244
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		public static IList GetVisualStateGroups(FrameworkElement obj)
		{
			if (obj == null)
			{
				throw new ArgumentNullException("obj");
			}
			return obj.GetValue(VisualStateManager.VisualStateGroupsProperty) as IList;
		}

		// Token: 0x06000CF9 RID: 3321 RVA: 0x00030064 File Offset: 0x0002E264
		internal static bool TryGetState(IList<VisualStateGroup> groups, string stateName, out VisualStateGroup group, out VisualState state)
		{
			for (int i = 0; i < groups.Count; i++)
			{
				VisualStateGroup visualStateGroup = groups[i];
				VisualState state2 = visualStateGroup.GetState(stateName);
				if (state2 != null)
				{
					group = visualStateGroup;
					state = state2;
					return true;
				}
			}
			group = null;
			state = null;
			return false;
		}

		// Token: 0x06000CFA RID: 3322 RVA: 0x000300A4 File Offset: 0x0002E2A4
		private static bool GoToStateInternal(FrameworkElement control, FrameworkElement stateGroupsRoot, VisualStateGroup group, VisualState state, bool useTransitions)
		{
			if (stateGroupsRoot == null)
			{
				throw new ArgumentNullException("stateGroupsRoot");
			}
			if (state == null)
			{
				throw new ArgumentNullException("state");
			}
			if (group == null)
			{
				throw new InvalidOperationException();
			}
			VisualState lastState = group.CurrentState;
			if (lastState == state)
			{
				return true;
			}
			VisualTransition transition = useTransitions ? VisualStateManager.GetTransition(stateGroupsRoot, group, lastState, state) : null;
			Storyboard storyboard = VisualStateManager.GenerateDynamicTransitionAnimations(stateGroupsRoot, group, state, transition);
			if (transition == null || (transition.GeneratedDuration == VisualStateManager.DurationZero && (transition.Storyboard == null || transition.Storyboard.Duration == VisualStateManager.DurationZero)))
			{
				if (transition != null && transition.Storyboard != null)
				{
					group.StartNewThenStopOld(stateGroupsRoot, new Storyboard[]
					{
						transition.Storyboard,
						state.Storyboard
					});
				}
				else
				{
					group.StartNewThenStopOld(stateGroupsRoot, new Storyboard[]
					{
						state.Storyboard
					});
				}
				group.RaiseCurrentStateChanging(stateGroupsRoot, lastState, state, control);
				group.RaiseCurrentStateChanged(stateGroupsRoot, lastState, state, control);
			}
			else
			{
				transition.DynamicStoryboardCompleted = false;
				storyboard.Completed += delegate(object sender, EventArgs e)
				{
					if (transition.Storyboard == null || transition.ExplicitStoryboardCompleted)
					{
						if (VisualStateManager.ShouldRunStateStoryboard(control, stateGroupsRoot, state, group))
						{
							group.StartNewThenStopOld(stateGroupsRoot, new Storyboard[]
							{
								state.Storyboard
							});
						}
						group.RaiseCurrentStateChanged(stateGroupsRoot, lastState, state, control);
					}
					transition.DynamicStoryboardCompleted = true;
				};
				if (transition.Storyboard != null && transition.ExplicitStoryboardCompleted)
				{
					EventHandler transitionCompleted = null;
					transitionCompleted = delegate(object sender, EventArgs e)
					{
						if (transition.DynamicStoryboardCompleted)
						{
							if (VisualStateManager.ShouldRunStateStoryboard(control, stateGroupsRoot, state, group))
							{
								group.StartNewThenStopOld(stateGroupsRoot, new Storyboard[]
								{
									state.Storyboard
								});
							}
							group.RaiseCurrentStateChanged(stateGroupsRoot, lastState, state, control);
						}
						transition.Storyboard.Completed -= transitionCompleted;
						transition.ExplicitStoryboardCompleted = true;
					};
					transition.ExplicitStoryboardCompleted = false;
					transition.Storyboard.Completed += transitionCompleted;
				}
				group.StartNewThenStopOld(stateGroupsRoot, new Storyboard[]
				{
					transition.Storyboard,
					storyboard
				});
				group.RaiseCurrentStateChanging(stateGroupsRoot, lastState, state, control);
			}
			group.CurrentState = state;
			return true;
		}

		// Token: 0x06000CFB RID: 3323 RVA: 0x00030360 File Offset: 0x0002E560
		[SecurityCritical]
		[SecurityTreatAsSafe]
		private static bool ShouldRunStateStoryboard(FrameworkElement control, FrameworkElement stateGroupsRoot, VisualState state, VisualStateGroup group)
		{
			bool flag = true;
			bool flag2 = true;
			if (control != null && !control.IsVisible)
			{
				flag = (PresentationSource.CriticalFromVisual(control) != null);
			}
			if (stateGroupsRoot != null && !stateGroupsRoot.IsVisible)
			{
				flag2 = (PresentationSource.CriticalFromVisual(stateGroupsRoot) != null);
			}
			return flag && flag2 && state == group.CurrentState;
		}

		/// <summary>Raises the <see cref="E:System.Windows.VisualStateGroup.CurrentStateChanging" /> event on the specified <see cref="T:System.Windows.VisualStateGroup" /> object.</summary>
		/// <param name="stateGroup">The object that the <see cref="E:System.Windows.VisualStateGroup.CurrentStateChanging" /> event occurred on.</param>
		/// <param name="oldState">The state that the control is transitioning from.</param>
		/// <param name="newState">The state that the control is transitioning to.</param>
		/// <param name="control">The control that is transitioning states.</param>
		/// <param name="stateGroupsRoot">The root element that contains the <see cref="T:System.Windows.VisualStateManager" />.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="stateGroupsRoot" /> is <see langword="null" />.-or-
		///         <paramref name="newState" /> is <see langword="null" />.</exception>
		// Token: 0x06000CFC RID: 3324 RVA: 0x000303AB File Offset: 0x0002E5AB
		protected void RaiseCurrentStateChanging(VisualStateGroup stateGroup, VisualState oldState, VisualState newState, FrameworkElement control, FrameworkElement stateGroupsRoot)
		{
			if (stateGroup == null)
			{
				throw new ArgumentNullException("stateGroup");
			}
			if (newState == null)
			{
				throw new ArgumentNullException("newState");
			}
			if (stateGroupsRoot == null)
			{
				return;
			}
			stateGroup.RaiseCurrentStateChanging(stateGroupsRoot, oldState, newState, control);
		}

		/// <summary>Raises the <see cref="E:System.Windows.VisualStateGroup.CurrentStateChanging" /> event on the specified <see cref="T:System.Windows.VisualStateGroup" /> object.</summary>
		/// <param name="stateGroup">The object that the <see cref="E:System.Windows.VisualStateGroup.CurrentStateChanging" /> event occurred on.</param>
		/// <param name="oldState">The state that the control is transitioning from.</param>
		/// <param name="newState">The state that the control is transitioning to.</param>
		/// <param name="control">The control that is transitioning states.</param>
		/// <param name="stateGroupsRoot">The root element that contains the <see cref="T:System.Windows.VisualStateManager" />.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="stateGroupsRoot" /> is <see langword="null" />.-or-
		///         <paramref name="newState" /> is <see langword="null" />.</exception>
		// Token: 0x06000CFD RID: 3325 RVA: 0x000303DA File Offset: 0x0002E5DA
		protected void RaiseCurrentStateChanged(VisualStateGroup stateGroup, VisualState oldState, VisualState newState, FrameworkElement control, FrameworkElement stateGroupsRoot)
		{
			if (stateGroup == null)
			{
				throw new ArgumentNullException("stateGroup");
			}
			if (newState == null)
			{
				throw new ArgumentNullException("newState");
			}
			if (stateGroupsRoot == null)
			{
				return;
			}
			stateGroup.RaiseCurrentStateChanged(stateGroupsRoot, oldState, newState, control);
		}

		// Token: 0x06000CFE RID: 3326 RVA: 0x0003040C File Offset: 0x0002E60C
		private static Storyboard GenerateDynamicTransitionAnimations(FrameworkElement root, VisualStateGroup group, VisualState newState, VisualTransition transition)
		{
			IEasingFunction easingFunction = null;
			Storyboard storyboard = new Storyboard();
			if (transition != null)
			{
				Duration generatedDuration = transition.GeneratedDuration;
				storyboard.Duration = transition.GeneratedDuration;
				easingFunction = transition.GeneratedEasingFunction;
			}
			else
			{
				storyboard.Duration = new Duration(TimeSpan.Zero);
			}
			Dictionary<VisualStateManager.TimelineDataToken, Timeline> dictionary = VisualStateManager.FlattenTimelines(group.CurrentStoryboards);
			Dictionary<VisualStateManager.TimelineDataToken, Timeline> dictionary2 = VisualStateManager.FlattenTimelines((transition != null) ? transition.Storyboard : null);
			Dictionary<VisualStateManager.TimelineDataToken, Timeline> dictionary3 = VisualStateManager.FlattenTimelines(newState.Storyboard);
			foreach (KeyValuePair<VisualStateManager.TimelineDataToken, Timeline> keyValuePair in dictionary2)
			{
				dictionary.Remove(keyValuePair.Key);
				dictionary3.Remove(keyValuePair.Key);
			}
			foreach (KeyValuePair<VisualStateManager.TimelineDataToken, Timeline> keyValuePair2 in dictionary3)
			{
				Timeline timeline = VisualStateManager.GenerateToAnimation(root, keyValuePair2.Value, easingFunction, true);
				if (timeline != null)
				{
					timeline.Duration = storyboard.Duration;
					storyboard.Children.Add(timeline);
				}
				dictionary.Remove(keyValuePair2.Key);
			}
			foreach (KeyValuePair<VisualStateManager.TimelineDataToken, Timeline> keyValuePair3 in dictionary)
			{
				Timeline timeline2 = VisualStateManager.GenerateFromAnimation(root, keyValuePair3.Value, easingFunction);
				if (timeline2 != null)
				{
					timeline2.Duration = storyboard.Duration;
					storyboard.Children.Add(timeline2);
				}
			}
			return storyboard;
		}

		// Token: 0x06000CFF RID: 3327 RVA: 0x000305B4 File Offset: 0x0002E7B4
		private static Timeline GenerateFromAnimation(FrameworkElement root, Timeline timeline, IEasingFunction easingFunction)
		{
			Timeline timeline2 = null;
			if (timeline is ColorAnimation || timeline is ColorAnimationUsingKeyFrames)
			{
				timeline2 = new ColorAnimation
				{
					EasingFunction = easingFunction
				};
			}
			else if (timeline is DoubleAnimation || timeline is DoubleAnimationUsingKeyFrames)
			{
				timeline2 = new DoubleAnimation
				{
					EasingFunction = easingFunction
				};
			}
			else if (timeline is PointAnimation || timeline is PointAnimationUsingKeyFrames)
			{
				timeline2 = new PointAnimation
				{
					EasingFunction = easingFunction
				};
			}
			if (timeline2 != null)
			{
				VisualStateManager.CopyStoryboardTargetProperties(root, timeline, timeline2);
			}
			return timeline2;
		}

		// Token: 0x06000D00 RID: 3328 RVA: 0x0003062C File Offset: 0x0002E82C
		private static Timeline GenerateToAnimation(FrameworkElement root, Timeline timeline, IEasingFunction easingFunction, bool isEntering)
		{
			Timeline timeline2 = null;
			Color? targetColor = VisualStateManager.GetTargetColor(timeline, isEntering);
			if (targetColor != null)
			{
				ColorAnimation colorAnimation = new ColorAnimation
				{
					To = targetColor,
					EasingFunction = easingFunction
				};
				timeline2 = colorAnimation;
			}
			if (timeline2 == null)
			{
				double? targetDouble = VisualStateManager.GetTargetDouble(timeline, isEntering);
				if (targetDouble != null)
				{
					DoubleAnimation doubleAnimation = new DoubleAnimation
					{
						To = targetDouble,
						EasingFunction = easingFunction
					};
					timeline2 = doubleAnimation;
				}
			}
			if (timeline2 == null)
			{
				Point? targetPoint = VisualStateManager.GetTargetPoint(timeline, isEntering);
				if (targetPoint != null)
				{
					PointAnimation pointAnimation = new PointAnimation
					{
						To = targetPoint,
						EasingFunction = easingFunction
					};
					timeline2 = pointAnimation;
				}
			}
			if (timeline2 != null)
			{
				VisualStateManager.CopyStoryboardTargetProperties(root, timeline, timeline2);
			}
			return timeline2;
		}

		// Token: 0x06000D01 RID: 3329 RVA: 0x000306C8 File Offset: 0x0002E8C8
		private static void CopyStoryboardTargetProperties(FrameworkElement root, Timeline source, Timeline destination)
		{
			if (source != null || destination != null)
			{
				string targetName = Storyboard.GetTargetName(source);
				DependencyObject dependencyObject = Storyboard.GetTarget(source);
				PropertyPath targetProperty = Storyboard.GetTargetProperty(source);
				if (dependencyObject == null && !string.IsNullOrEmpty(targetName))
				{
					dependencyObject = (root.FindName(targetName) as DependencyObject);
				}
				if (targetName != null)
				{
					Storyboard.SetTargetName(destination, targetName);
				}
				if (dependencyObject != null)
				{
					Storyboard.SetTarget(destination, dependencyObject);
				}
				if (targetProperty != null)
				{
					Storyboard.SetTargetProperty(destination, targetProperty);
				}
			}
		}

		// Token: 0x06000D02 RID: 3330 RVA: 0x00030728 File Offset: 0x0002E928
		internal static VisualTransition GetTransition(FrameworkElement element, VisualStateGroup group, VisualState from, VisualState to)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			if (group == null)
			{
				throw new ArgumentNullException("group");
			}
			if (to == null)
			{
				throw new ArgumentNullException("to");
			}
			VisualTransition visualTransition = null;
			VisualTransition visualTransition2 = null;
			int num = -1;
			IList<VisualTransition> list = (IList<VisualTransition>)group.Transitions;
			if (list != null)
			{
				foreach (VisualTransition visualTransition3 in list)
				{
					if (visualTransition2 == null && visualTransition3.IsDefault)
					{
						visualTransition2 = visualTransition3;
					}
					else
					{
						int num2 = -1;
						VisualState state = group.GetState(visualTransition3.From);
						VisualState state2 = group.GetState(visualTransition3.To);
						if (from == state)
						{
							num2++;
						}
						else if (state != null)
						{
							continue;
						}
						if (to == state2)
						{
							num2 += 2;
						}
						else if (state2 != null)
						{
							continue;
						}
						if (num2 > num)
						{
							num = num2;
							visualTransition = visualTransition3;
						}
					}
				}
			}
			return visualTransition ?? visualTransition2;
		}

		// Token: 0x06000D03 RID: 3331 RVA: 0x00030818 File Offset: 0x0002EA18
		private static Color? GetTargetColor(Timeline timeline, bool isEntering)
		{
			ColorAnimation colorAnimation = timeline as ColorAnimation;
			if (colorAnimation != null)
			{
				if (colorAnimation.From == null)
				{
					return colorAnimation.To;
				}
				return colorAnimation.From;
			}
			else
			{
				ColorAnimationUsingKeyFrames colorAnimationUsingKeyFrames = timeline as ColorAnimationUsingKeyFrames;
				if (colorAnimationUsingKeyFrames == null)
				{
					return null;
				}
				if (colorAnimationUsingKeyFrames.KeyFrames.Count == 0)
				{
					return null;
				}
				ColorKeyFrame colorKeyFrame = colorAnimationUsingKeyFrames.KeyFrames[isEntering ? 0 : (colorAnimationUsingKeyFrames.KeyFrames.Count - 1)];
				return new Color?(colorKeyFrame.Value);
			}
		}

		// Token: 0x06000D04 RID: 3332 RVA: 0x000308A4 File Offset: 0x0002EAA4
		private static double? GetTargetDouble(Timeline timeline, bool isEntering)
		{
			DoubleAnimation doubleAnimation = timeline as DoubleAnimation;
			if (doubleAnimation != null)
			{
				if (doubleAnimation.From == null)
				{
					return doubleAnimation.To;
				}
				return doubleAnimation.From;
			}
			else
			{
				DoubleAnimationUsingKeyFrames doubleAnimationUsingKeyFrames = timeline as DoubleAnimationUsingKeyFrames;
				if (doubleAnimationUsingKeyFrames == null)
				{
					return null;
				}
				if (doubleAnimationUsingKeyFrames.KeyFrames.Count == 0)
				{
					return null;
				}
				DoubleKeyFrame doubleKeyFrame = doubleAnimationUsingKeyFrames.KeyFrames[isEntering ? 0 : (doubleAnimationUsingKeyFrames.KeyFrames.Count - 1)];
				return new double?(doubleKeyFrame.Value);
			}
		}

		// Token: 0x06000D05 RID: 3333 RVA: 0x00030930 File Offset: 0x0002EB30
		private static Point? GetTargetPoint(Timeline timeline, bool isEntering)
		{
			PointAnimation pointAnimation = timeline as PointAnimation;
			if (pointAnimation != null)
			{
				if (pointAnimation.From == null)
				{
					return pointAnimation.To;
				}
				return pointAnimation.From;
			}
			else
			{
				PointAnimationUsingKeyFrames pointAnimationUsingKeyFrames = timeline as PointAnimationUsingKeyFrames;
				if (pointAnimationUsingKeyFrames == null)
				{
					return null;
				}
				if (pointAnimationUsingKeyFrames.KeyFrames.Count == 0)
				{
					return null;
				}
				PointKeyFrame pointKeyFrame = pointAnimationUsingKeyFrames.KeyFrames[isEntering ? 0 : (pointAnimationUsingKeyFrames.KeyFrames.Count - 1)];
				return new Point?(pointKeyFrame.Value);
			}
		}

		// Token: 0x06000D06 RID: 3334 RVA: 0x000309BC File Offset: 0x0002EBBC
		private static Dictionary<VisualStateManager.TimelineDataToken, Timeline> FlattenTimelines(Storyboard storyboard)
		{
			Dictionary<VisualStateManager.TimelineDataToken, Timeline> result = new Dictionary<VisualStateManager.TimelineDataToken, Timeline>();
			VisualStateManager.FlattenTimelines(storyboard, result);
			return result;
		}

		// Token: 0x06000D07 RID: 3335 RVA: 0x000309D8 File Offset: 0x0002EBD8
		private static Dictionary<VisualStateManager.TimelineDataToken, Timeline> FlattenTimelines(Collection<Storyboard> storyboards)
		{
			Dictionary<VisualStateManager.TimelineDataToken, Timeline> result = new Dictionary<VisualStateManager.TimelineDataToken, Timeline>();
			for (int i = 0; i < storyboards.Count; i++)
			{
				VisualStateManager.FlattenTimelines(storyboards[i], result);
			}
			return result;
		}

		// Token: 0x06000D08 RID: 3336 RVA: 0x00030A0C File Offset: 0x0002EC0C
		private static void FlattenTimelines(Storyboard storyboard, Dictionary<VisualStateManager.TimelineDataToken, Timeline> result)
		{
			if (storyboard == null)
			{
				return;
			}
			for (int i = 0; i < storyboard.Children.Count; i++)
			{
				Timeline timeline = storyboard.Children[i];
				Storyboard storyboard2 = timeline as Storyboard;
				if (storyboard2 != null)
				{
					VisualStateManager.FlattenTimelines(storyboard2, result);
				}
				else
				{
					result[new VisualStateManager.TimelineDataToken(timeline)] = timeline;
				}
			}
		}

		/// <summary>Identifies the <see cref="P:System.Windows.VisualStateManager.CustomVisualStateManager" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.VisualStateManager.CustomVisualStateManager" /> dependency property.</returns>
		// Token: 0x04000B31 RID: 2865
		public static readonly DependencyProperty CustomVisualStateManagerProperty = DependencyProperty.RegisterAttached("CustomVisualStateManager", typeof(VisualStateManager), typeof(VisualStateManager), null);

		// Token: 0x04000B32 RID: 2866
		private static readonly DependencyPropertyKey VisualStateGroupsPropertyKey = DependencyProperty.RegisterAttachedReadOnly("VisualStateGroups", typeof(IList), typeof(VisualStateManager), new FrameworkPropertyMetadata(new ObservableCollectionDefaultValueFactory<VisualStateGroup>()));

		/// <summary>Identifies the <see cref="P:System.Windows.VisualStateManager.VisualStateGroups" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.VisualStateManager.VisualStateGroups" /> dependency property.</returns>
		// Token: 0x04000B33 RID: 2867
		public static readonly DependencyProperty VisualStateGroupsProperty = VisualStateManager.VisualStateGroupsPropertyKey.DependencyProperty;

		// Token: 0x04000B34 RID: 2868
		private static readonly Duration DurationZero = new Duration(TimeSpan.Zero);

		// Token: 0x02000832 RID: 2098
		private struct TimelineDataToken : IEquatable<VisualStateManager.TimelineDataToken>
		{
			// Token: 0x06007EB7 RID: 32439 RVA: 0x002368F2 File Offset: 0x00234AF2
			public TimelineDataToken(Timeline timeline)
			{
				this._target = Storyboard.GetTarget(timeline);
				this._targetName = Storyboard.GetTargetName(timeline);
				this._targetProperty = Storyboard.GetTargetProperty(timeline);
			}

			// Token: 0x06007EB8 RID: 32440 RVA: 0x00236918 File Offset: 0x00234B18
			public bool Equals(VisualStateManager.TimelineDataToken other)
			{
				bool flag;
				if (this._targetName != null)
				{
					flag = (other._targetName == this._targetName);
				}
				else if (this._target != null)
				{
					flag = (other._target == this._target);
				}
				else
				{
					flag = (other._target == null && other._targetName == null);
				}
				if (flag && other._targetProperty.Path == this._targetProperty.Path && other._targetProperty.PathParameters.Count == this._targetProperty.PathParameters.Count)
				{
					bool result = true;
					int i = 0;
					int count = this._targetProperty.PathParameters.Count;
					while (i < count)
					{
						if (other._targetProperty.PathParameters[i] != this._targetProperty.PathParameters[i])
						{
							result = false;
							break;
						}
						i++;
					}
					return result;
				}
				return false;
			}

			// Token: 0x06007EB9 RID: 32441 RVA: 0x00236A00 File Offset: 0x00234C00
			public override int GetHashCode()
			{
				int num = (this._target != null) ? this._target.GetHashCode() : 0;
				int num2 = (this._targetName != null) ? this._targetName.GetHashCode() : 0;
				int num3 = (this._targetProperty != null && this._targetProperty.Path != null) ? this._targetProperty.Path.GetHashCode() : 0;
				return ((this._targetName != null) ? num2 : num) ^ num3;
			}

			// Token: 0x04003CB7 RID: 15543
			private DependencyObject _target;

			// Token: 0x04003CB8 RID: 15544
			private string _targetName;

			// Token: 0x04003CB9 RID: 15545
			private PropertyPath _targetProperty;
		}
	}
}
