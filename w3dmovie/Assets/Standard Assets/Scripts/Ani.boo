import UnityEngine

# AniMate animation helper class for Unity3D
# Version 2.0 - 9. October 2009
# Copyright (C) 2009  Adrian Stutz
# 
# This library is free software; you can redistribute it and/or
# modify it under the terms of the GNU Lesser General Public
# License as published by the Free Software Foundation; either
# version 2.1 of the License, or (at your option) any later version.
# 
# This library is distributed in the hope that it will be useful,
# but WITHOUT ANY WARRANTY; without even the implied warranty of
# MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
# Lesser General Public License for more details.
# 
# You should have received a copy of the GNU Lesser General Public
# License along with this library; if not, see <http://www.gnu.org/licenses/>.

import System
import System.Reflection
import System.Reflection.Emit
import System.Collections

# ---------------------------------------- #
# ANIMATION HELPER CLASS

class Ani (MonoBehaviour): 
	
	enum AniType:
		To
		From
		By
	
	enum Message:
		Quiet
		Error
		Warning
		Verbose
		MoreVerbose
	
	# ---------------------------------------- #
	# CONFIGURATION PROPERTIES
	
	# Default delay
	public defaultDelay as single = 0
	# Default physics behaviour
	public defaultPhysics as bool = false
	# Default callback
	public defaultCallback as callable = null
	# Default easing
	public defaultEasing as Type = Easing.Linear
	# Default easing direction
	public defaultDirection as Easing.Direction = Easing.In
	# Default animation drive
	public defaultDrive as Type = Drive.Regular
	# Default frames per second (-1 for fullspeed)
	public defaultFps as single = -1
	# Remove existing animations for property
	public defaultReplace as bool = false
	
	# AniMate debug level
	public debug as Message = Message.Warning
	
	# ---------------------------------------- #
	# INTERNAL FIELDS
	
	# List with of AniProp classes
	animations as List = []
	fixedAnimations as List = []
	
	lastLevel as Message
	
	# ---------------------------------------- #
	# SINGLETON
	
	# Singleton instance
	static Mate as Ani:
		get:
			# Create instance if none exists yet
			if _mate == null:
				# Create GameObject to attach to
				go = GameObject("AniMate")
				# Attach Ani to GameObject
				_mate = go.AddComponent(Ani)
				
				_mate.Log("Created game object 'AniMate' to initialize "+
							"AniMate instance.") if _mate.Level(Message.MoreVerbose)
			return _mate
	static _mate as Ani
	
	# Save instance
	def Awake():
		if Ani._mate:
			Log("Multiple AniMate instances on "+Ani._mate.gameObject.name+
					" and "+gameObject.name) if Level(Message.Error)
			return
		Ani._mate = self
	
	# ---------------------------------------- #
	# Debug Message Helper method
	
	# Logging is used with following syntax to reduce overhead:
	# Log("log message") if Level(Message.LEVEL)
	
	protected def Log(message as string):
		Debug.Log("Ani.Mate."+lastLevel+": "+message)
	
	protected def Level(level as Message):
		lastLevel = level
		return cast(int,level) <= cast(int,debug)
	
	# ---------------------------------------- #
	# CREATE NEW ANIMATION
	
	def To (obj as object, duration as single, properties as Hash):
		properties = properties.Clone()
		# Fill options with defaults
		options = ExtractOptions(properties)
		# Add to animations
		CreateAnimations(obj, properties, duration, options, AniType.To)
		# Return yield to chain animations
		return WaitForSeconds(duration)
	
	def To (obj as object, duration as single, properties as Hashtable):
		To (obj, duration, Hash(properties))
	
	def To (obj as object, duration as single, properties as Hash, options as Hash):
		properties = properties.Clone()
		options = options.Clone()
		# Fill options with defaults
		options = ExtractOptions(options)
		# Add to animations
		CreateAnimations(obj, properties, duration, options, AniType.To)
		# Return yield to chain animations
		return WaitForSeconds(duration)
	
	def To (obj as object, duration as single, properties as Hashtable, options as Hashtable):
		To (obj, duration, Hash(properties), Hash(options))
	
	def From (obj as object, duration as single, properties as Hash):
		properties = properties.Clone()
		# Fill options with defaults
		options = ExtractOptions(properties)
		# Add to animations
		CreateAnimations(obj, properties, duration, options, AniType.From)
		# Return yield to chain animations
		return WaitForSeconds(duration)
	
	def From (obj as object, duration as single, properties as Hashtable):
		From (obj, duration, Hash(properties))
	
	def From (obj as object, duration as single, properties as Hash, options as Hash):
		properties = properties.Clone()
		options = options.Clone()
		# Fill options with defaults
		options = ExtractOptions(options)
		# Add to animations
		CreateAnimations(obj, properties, duration, options, AniType.From)
		# Return yield to chain animations
		return WaitForSeconds(duration)
	
	def From (obj as object, duration as single, properties as Hashtable, options as Hashtable):
		From (obj, duration, Hash(properties), Hash(options))
	
	def By (obj as object, duration as single, properties as Hash):
		properties = properties.Clone()
		# Fill options with defaults
		options = ExtractOptions(properties)
		# Add to animations
		CreateAnimations(obj, properties, duration, options, AniType.By)
		# Return yield to chain animations
		return WaitForSeconds(duration)
	
	def By (obj as object, duration as single, properties as Hashtable):
		By (obj, duration, Hash(properties))
	
	def By (obj as object, duration as single, properties as Hash, options as Hash):
		properties = properties.Clone()
		options = options.Clone()
		# Fill options with defaults
		options = ExtractOptions(options)
		# Add to animations
		CreateAnimations(obj, properties, duration, options, AniType.By)
		# Return yield to chain animations
		return WaitForSeconds(duration)
	
	def By (obj as object, duration as single, properties as Hashtable, options as Hashtable):
		By (obj, duration, Hash(properties), Hash(options))
	
	# ---------------------------------------- #
	# MANAGE ANIMATIONS
	
	# Number of all aniamtions
	def Count():
		return (animations.Count + fixedAnimations.Count)
	
	# Check if an animation exists for object
	def Has(obj as object):
		return (Contains(obj,null,animations) or Contains(obj,null,fixedAnimations))
	
	# Check if animation exists for object and proeperty
	def Has(obj as object, pName as string):
		return (Contains(obj,pName,animations) or Contains(obj,pName,fixedAnimations))
	
	# Check for object and property
	protected def Contains(obj as object, pName as string, anims as List):
		for anim as AniProp in anims:
			if ((pName == null and anim.value.Is(obj))
					or
				(pName != null and anim.value.Is(obj,pName))):
				return true
		return false
	
	# Cancel all aniamtions on an object
	def Cancel(obj as object):
		Cancel(obj,null)
	
	# Cancel animation (set to initial value)
	def Cancel(obj as object, pName as string):
		toCancel = GetAnimations(obj,pName)
		for anim as AniProp in toCancel:
			Apply(0,anim,true)
			animations.Remove(anim)
			fixedAnimations.Remove(anim)
			Log("Manually cancelled animation on "+anim.value.Object+"."+anim.value.Name) if Level(Message.Verbose)
	
	# Finish all aniamtions on an object
	def Finish(obj as object):
		Finish(obj,null)
	
	# Finish animation (set to end value)
	def Finish(obj as object, pName as string):
		toFinish = GetAnimations(obj,pName)
		for anim as AniProp in toFinish:
			Apply(1,anim,true)
			animations.Remove(anim)
			fixedAnimations.Remove(anim)
			Log("Manually finished animation on "+anim.value.Object+"."+anim.value.Name) if Level(Message.Verbose)
	
	# Stop all animations of an object
	def StopAll(obj as object):
		Stop(obj,null,null)
	def Stop(obj as object):
		Stop(obj,null,null)
	
	# Stop all animations of an object for a property
	def Stop(obj as object, pName as string):
		Stop(obj,pName,null)
	
	protected def Stop(obj as object, pName as string, exclude as AniProp):
		toStop = GetAnimations(obj,pName)
		for anim as AniProp in toStop:
			continue if exclude == anim
			animations.Remove(anim)
			fixedAnimations.Remove(anim)
			Log("Manually stopped animation on "+anim.value.Object+"."+anim.value.Name) if Level(Message.Verbose)
	
	# Get Animation Lists
	protected def GetAnimations(obj as object, pName as string):
		hits = []
		# Look in regular animations
		for anim as AniProp in animations:
			if ((pName == null and anim.value.Is(obj))
					or
				(pName != null and anim.value.Is(obj,pName))):
				hits.Add(anim)
		# Look in fixed animations
		for anim as AniProp in fixedAnimations:
			if ((pName == null and anim.value.Is(obj))
					or
				(pName != null and anim.value.Is(obj,pName))):
				hits.Add(anim)
		return hits
	
	# ---------------------------------------- #
	# MAIN ANIMATION LOOPS
	
	protected def DoAnimation(anims as List):
		finished = []
		# Loop through animations
		for anim as AniProp in anims:
			# Check for delayed animation
			if not anim.mator:
				if anim.startTime <= Time.time:
					StartAnimation(anim)
				else:
					continue
			# Apply animation
			if not Apply(anim.mator.GetPosition(),anim,false):
				finished.Add(anim)
		# Remove finished animations
		for fin in finished:
			anims.Remove(fin)
	
	protected def Apply(position as single, anim as AniProp, forceUpdate as bool):
		spf = cast(single,anim.options["fps"])
		# Ignore restrictions if forced
		if not forceUpdate:
			# Honor seconds per frame
			if spf > 0:
				anim.timeSinceLastFrame += Time.deltaTime
				# Not yet time, skip
				if anim.timeSinceLastFrame < spf:
					return true
				# Update this frame
				else:
					anim.timeSinceLastFrame = anim.timeSinceLastFrame % spf
		# Animate or call calback with value
		try:
			Log("Update "+anim.value.Object+"."+anim.value.Name+
					" to "+anim.mator.GetValue(position)) if Level(Message.MoreVerbose)
			if not anim.callback:
				anim.value.Set(anim.mator.GetValue(position))
			else:
				anim.callback(anim.mator.GetValue(position))
		except e as Exception:
			Log("Animation stopped because of exception: "+e) if Level(Message.Warning)
			return false
		# Check if finished
		if anim.mator.Finished():
			Log("Finished animation on "+anim.value.Object+"."+anim.value.Name) if Level(Message.Verbose)
			return false
		return true
	
	# Regular animations
	def Update():
		DoAnimation(animations)
	
	# Physics animations
	def FixedUpdate():
		DoAnimation(fixedAnimations)
	
	# ---------------------------------------- #
	# INTERNAL METHODS
	
	# Exctract options for Hash and fill defaults where needed
	protected def ExtractOptions(options as Hash):
		exct = {}
		# Delay
		if (options["delay"] == null):
			exct["delay"] = defaultDelay
		else:
			exct["delay"] = cast(single,options["delay"])
			options.Remove("delay")
		# Physics
		if (options["physics"] == null):
			exct["physics"] = defaultPhysics
		else:
			exct["physics"] = cast(bool,options["physics"])
			options.Remove("physics")
		# Callback
		if (options["callback"] == null):
			exct["callback"] = defaultCallback
		else:
			exct["callback"] = cast(callable,options["callback"])
			options.Remove("callback")
		# Easing
		if (options["easing"] == null):
			exct["easing"] = defaultEasing
		else:
			exct["easing"] = cast(Type,options["easing"])
			options.Remove("easing")
		# Easing Direction
		if (options["direction"] == null):
			exct["direction"] = defaultDirection
		else:
			exct["direction"] = cast(Easing.Direction,options["direction"])
			options.Remove("direction")
		# Animation drive
		if (options["drive"] == null):
			exct["drive"] = defaultDrive
		else:
			exct["drive"] = cast(Type,options["drive"])
			options.Remove("drive")
		# Rigidbody animation
		if (options["rigidbody"] == null):
			exct["rigidbody"] = null
		else:
			exct["rigidbody"] = cast(Rigidbody,options["rigidbody"])
			options.Remove("rigidbody")
		# Color animation
		if (options["colorName"] == null):
			exct["colorName"] = null
		else:
			exct["colorName"] = cast(string,options["colorName"])
			options.Remove("colorName")
		# Fps (saved as seconds per frame)
		if (options["fps"] == null):
			exct["fps"] = 1 / defaultFps
		else:
			exct["fps"] = 1 / cast(single,options["fps"])
			options.Remove("fps")
		# Replace animation on property
		if (options["replace"] == null):
			exct["replace"] = defaultReplace
		else:
			exct["replace"] = cast(bool,options["replace"])
			options.Remove("replace")
		# Return hash with all values
		return exct
	
	# Extract animation properties from Hash
	protected def CreateAnimations(obj as object, properties as Hash, duration as single, options as Hash, type as AniType):
		for item in properties:
			# Extract name and value
			pName = cast(string,item.Key)
			# Create value object
			if options["colorName"]:
				# Special handling for material color animation: pass color instead of material
				aniv = AniValue((obj as Material).GetColor(options["colorName"]),pName)
			else:
				aniv = AniValue(obj,pName)
			# Check for error (proeprty not found)
			return unless aniv.Setter
			# Cast value to destination type
			argument as duck = Convert.ChangeType(item.Value, aniv.ValueType())
			# Callback
			callback as callable = options["callback"]
			# Animation type
			isFixedAnimation as bool = false
			# Material color animtion
			if (options["colorName"]):
				if not obj isa Material:
					Log("colorName can only be set on material objects.") if Level(Message.Error)
					return
				if not pName in ["r","g","b","a"]:
					Log("colorName can only be used with r, g, b and a.") if Level(Message.Error)
					return
				callback = CreateMaterialColorCallback(obj,pName,options)
			# Physics / Rigidbody animtion
			if (options["physics"] == true or options["rigidbody"] != null):
				# Animation will run in FixedUpdate
				isFixedAnimation = true
				# Rigidbody animation
				if options["rigidbody"] != null and pName == "position":
					callback = (options["rigidbody"] as Rigidbody).MovePosition
				elif options["rigidbody"] != null and pName == "rotation":
					callback = (options["rigidbody"] as Rigidbody).MoveRotation
			# Print warning if callback was overwritten
			if (options["callback"] != null and options["callback"] != callback):
				Log("callback option overwritten by rigidbody option.") if Level(Message.Warning)
			# Start time
			startTime as single = 0
			delay as single = options["delay"]
			if (delay > 0):
				startTime = Time.time + delay
			# Add animation to main list
			anim = AniProp(aniv, null, type, duration, callback, startTime, argument, options)
			if not isFixedAnimation:
				# Regular animation
				animations.Add(anim)
			else:
				# Physics animation
				fixedAnimations.Add(anim)
			Log("Created Animation on "+anim.value.Object+"."+anim.value.Name+" of type "+anim.type
					+" over "+anim.duration+"s and with "+delay+"s delay.") if Level(Message.Verbose)
			# Start animation if there's no delay
			if (delay == 0):
				StartAnimation(anim)
	
	protected def StartAnimation(anim as AniProp):
		# Get current value
		current = anim.value.Get()
		# Setup variables
		if (anim.type == AniType.To):
			start = current
			target = anim.argument
		elif (anim.type == AniType.From):
			start = anim.argument
			target = current
		elif (anim.type == AniType.By):
			start = current
			diff = anim.argument
		# Calculate difference for To and From
		if ((anim.type == AniType.To or anim.type == AniType.From) 
				and DriveNeedsDiff(anim.options["drive"])):
			try:
				diff = target - start
				if (start + 0.1 * diff) == self:
					pass
			except e as Exception:
				Log("Cannot animate "+anim.value.ValueType()+" with target "+anim.argument.GetType()+
						": Operation +, - or * not supported. ("+e+")") if Level(Message.Error)
				animations.Remove(anim)
				fixedAnimations.Remove(anim)
				return
		# Create animation object
		mat = AniMator(start, target, diff, anim.duration, anim.options["easing"], 
						anim.options["direction"], anim.options["drive"])
		# From: Set to starting value
		if (anim.type == AniType.From):
			anim.value.Set(mat.GetValue())
		# Remove existing animations
		if (anim.options["replace"]):
			Stop(anim.value.Object,anim.value.Name,anim)
		# Add AniMator to animation
		anim.mator = mat
		Log("Started animation on "+anim.value.Object+"."+anim.value.Name) if Level(Message.Verbose)
	
	protected def DriveNeedsDiff(drive as Type):
		d as AnimationDrive = drive()
		return d.CalculateDiff()
	
	protected def CreateMaterialColorCallback(obj as Material, pName as string, options as Hash):
		return def (newValue as single):
			material = (obj as Material)
			newColor = material.GetColor(options["colorName"])
			if pName == "r":
				newColor.r = newValue
			elif pName == "g":
				newColor.g = newValue
			elif pName == "b":
				newColor.b = newValue
			else:
				newColor.a = newValue
			material.SetColor(options["colorName"],newColor)
	
	# ---------------------------------------- #
	# CONTAINER CLASS FOR ANIMATION PROPERTIES
	
	protected class AniProp:
		public value as AniValue
		public mator as AniMator
		public type  as AniType
		public duration as single
		public callback as callable
		
		public timeSinceLastFrame as single
		public argument as duck
		public startTime as single
		public options as Hash
		
		def constructor(v as AniValue, m as AniMator, t as AniType, d as single, 
						c as callable, s as single, a as duck, o as Hash):
			value = v
			mator = m
			type = t
			duration = d
			callback = c
			startTime = s
			argument = a
			options = o
	
	# ---------------------------------------- #
	# WRAPPER FOR A SINGLE VALUE
	
	protected class AniValue:
	
		# ---------------------------------------- #
		# CONFIGURATION
	
		static bFlags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static
	
		callable SetHandler(source as object, value as object) as void
	
		# ---------------------------------------- #
		# PRIVATE FIELDS
	
		# Object a field or property is animated on
		[Getter(Object)]
		obj as object
		# Name of the field or property
		[Getter(Name)]
		name as string
		
		# Type object
		objType as Type
		# FieldInfo object
		fieldInfo as FieldInfo
		# PropertyInfo object
		propertyInfo as PropertyInfo
		
		[Getter(Setter)]
		setter as SetHandler
	
		# ---------------------------------------- #
		# CONSTRUCTOR
	
		def constructor(o as object, n as string):
			# Save
			obj = o
			name = n
			# Get info objects
			objType = obj.GetType()
			# Get field or property info
			fieldInfo = objType.GetField(n, AniValue.bFlags)
			propertyInfo = objType.GetProperty(n, AniValue.bFlags)
			# Check info objects
			if (fieldInfo == null and propertyInfo == null):
				Ani.Mate.Log("Property or field '"+n+"' not found on "+obj) if Ani.Mate.Level(Message.Error)
				return
			# Create dynamic method
			if fieldInfo:
				setter = FieldSetMethod(objType, fieldInfo)
			else:
				setter = PropertySetMethod(objType, propertyInfo)
	
		# ---------------------------------------- #
		# UTILITY METHODS
	
		# Get type of field/property for debug purposes
		def ValueType():
			if (propertyInfo != null):
				return propertyInfo.PropertyType
			else:
				return fieldInfo.FieldType
	
		# Check if AniValue is from given object
		def Is(checkObj as object):
			return (obj == checkObj)
	
		# Check if AniValue is from given object and value
		def Is(checkObject as object, checkName as string):
			return (Is(checkObject) and checkName == name)
	
		# Create a dynamic set method for a PropertyInfo
		def PropertySetMethod(type as Type, pInfo as PropertyInfo):
			setMethodInfo = pInfo.GetSetMethod(true)
			dynamicSet = DynamicMethod("SetHandler", void, (object, object), type, true)
			setGenerator = dynamicSet.GetILGenerator()
		
			setGenerator.Emit(OpCodes.Ldarg_0)
			setGenerator.Emit(OpCodes.Ldarg_1)
			if (setMethodInfo.GetParameters()[0].ParameterType.IsValueType):
				setGenerator.Emit(OpCodes.Unbox_Any, setMethodInfo.GetParameters()[0].ParameterType)
			setGenerator.Emit(OpCodes.Call, setMethodInfo)
			setGenerator.Emit(OpCodes.Ret)
		
			return dynamicSet.CreateDelegate(SetHandler) as SetHandler
	
		# Create a dynamic set method for a FieldInfo
		def FieldSetMethod(type as Type, fInfo as FieldInfo):
			dynamicSet = DynamicMethod("SetHandler", void, (object, object), type, true)
			setGenerator = dynamicSet.GetILGenerator()
		
			setGenerator.Emit(OpCodes.Ldarg_0)
			setGenerator.Emit(OpCodes.Ldarg_1)
			if (fInfo.FieldType.IsValueType):
				setGenerator.Emit(OpCodes.Unbox_Any, fInfo.FieldType)
			setGenerator.Emit(OpCodes.Stfld, fInfo)
			setGenerator.Emit(OpCodes.Ret)
		
			return dynamicSet.CreateDelegate(SetHandler) as SetHandler
	
		# ---------------------------------------- #
		# GET AND SET VALUE
	
		# Get field or property
		def Get():
			if (propertyInfo != null):
				return propertyInfo.GetValue(obj, null)
			else:
				return fieldInfo.GetValue(obj)
	
		# Set field or property
		def Set(value):
			if setter:
				setter(obj, value)
			elif (propertyInfo != null):
				propertyInfo.SetValue(obj, value, null)
			else:
				fieldInfo.SetValue(obj, value)
	
	# ---------------------------------------- #
	# ANIMATOR CLASS
	
	protected class AniMator:
	
		# Initial value
		startValue as duck
		# End value
		endValue as duck
		# Change over duration
		change as duck
	
		# Time of animation start
		startTime as single
		# Length of animation
		duration as single
		# Easing class
		easing as AnimationEasing
		# Easing type
		easingType as Easing.Direction
		# Animation drive
		drive as AnimationDrive
	
		# Fallback with dynamic typing
		def constructor(sta as duck, end as duck, chg as duck, dur as single, eas as Type, typ as Easing.Direction, d as Type):
			startValue = sta
			endValue = end
			change = chg
			Setup(dur, eas, typ, d)
	
		# Create Animator
		protected def Setup(dur as single, eas as Type, typ as Easing.Direction, d as Type):
			startTime = Time.time
			duration = dur
			easing = eas()
			easingType = typ
			drive = d()
	
		# Get easing with correct type
		def GetEasing(time as single):
			if easingType == Easing.In:
				return easing.In(time)
			elif easingType == Easing.Out:
				return easing.Out(time)
			elif easingType == Easing.InOut:
				return easing.InOut(time)
	
		# Get current animation position (from 0 to 1)
		def GetPosition():
			return GetEasing(Mathf.Clamp01((Time.time - startTime) / duration))

		# Check if aniamtion is finished
		def Finished():
			return (startTime + duration) <= Time.time
	
		# Get value for custom position
		def GetValue(easPos as single):
			# Use drive to calculate value
			return drive.Animate(startValue, endValue, change, easPos * duration, duration)
	
		# Get current animation value
		def GetValue():
			# Get eased position
			easPos = GetPosition()
			return GetValue(easPos)

	# ---------------------------------------- #
	# INTERFACE FOR ANIMATION DRIVES

	interface AnimationDrive:
		def Animate(start as duck, end as duck, diff as duck, time as single, duration as single) as duck
		def CalculateDiff () as bool

	# ---------------------------------------- #
	# REGULAR DRIVE
	
	class Drive: 
	
		class Regular (AnimationDrive):	
			def Animate(start as duck, end as duck, diff as duck, time as single, duration as single) as duck:
				# Positon
				easPos = time / duration
				# Cast to known types for performance
				startType = start.GetType()
				# --- Builtin types
				# Not matching start and change
				if (startType != diff.GetType()):
					return start + easPos * diff
				# short
				elif (startType == short):
					return cast(short,cast(short,start) + easPos * cast(short,diff))
				# integer
				elif (startType == int):
					return cast(int,cast(int,start) + easPos * cast(int,diff))
				# long
				elif (startType == long):
					return cast(long,cast(long,start) + easPos * cast(long,diff))
				# single
				elif (startType == single):
					return cast(single,cast(single,start) + easPos * cast(single,diff))
				# double
				elif (startType == double):
					return cast(double,cast(double,start) + easPos * cast(double,diff))
				# decimal
				elif (startType == decimal):
					return cast(decimal,cast(decimal,start) + easPos * cast(decimal,diff))
				# --- Unity types
				# Vector2
				elif (startType == Vector2):
					return cast(Vector2,start) + easPos * cast(Vector2,diff)
				# Vector3
				elif (startType == Vector3):
					return cast(Vector3,start) + easPos * cast(Vector3,diff)
				# Vector3
				elif (startType == Vector4):
					return cast(Vector4,start) + easPos * cast(Vector4,diff)
				# Color
				elif (startType == Color):
					return cast(Color,start) + easPos * cast(Color,diff)
				# Dynamic typed fallback
				else:
					return start + easPos * diff
	
			def CalculateDiff() as bool:
				return true

		# ---------------------------------------- #
		# SLERP DRIVE

		class Slerp (AnimationDrive):
			def Animate(start as duck, end as duck, diff as duck, time as single, duration as single) as duck:
				return Quaternion.Slerp(start, end, (time / duration))
			def CalculateDiff() as bool:
				return false

		# ---------------------------------------- #
		# LERP DRIVE

		class Lerp (AnimationDrive):
			def Animate(start as duck, end as duck, diff as duck, time as single, duration as single) as duck:
				return Vector3.Lerp(start, end, (time / duration))
			def CalculateDiff() as bool:
				return false

	# ---------------------------------------- #
	# INTERFACE FOR EASING FUNCTIONS

	interface AnimationEasing:
		def In (time as single) as single
		def Out (time as single) as single
		def InOut (time as single) as single

	protected class EasingHelper:
		static def InOut(eas as AnimationEasing, time as single) as single:
			if time <= .5:
				return eas.In(time * 2) / 2
			else:
				return (eas.Out((time - .5) * 2) / 2) + .5

	class Easing:
		
		enum Direction:
			In
			Out
			InOut
		
		public static final In = Direction.In
		public static final Out = Direction.Out
		public static final InOut = Direction.InOut
		
		# ---------------------------------------- #
		# LINEAR EASING

		class Linear (AnimationEasing):
			def In (time as single) as single:
				return time
			def Out (time as single) as single:
				return time
			def InOut (time as single):
				return time

		# ---------------------------------------- #
		# QUADRATIC EASING

		class Quadratic (AnimationEasing):
			def In (time as single) as single:
				return (time * time)
			def Out (time as single) as single:
				return (time * (time - 2) * -1)
			def InOut (time as single) as single:
				return EasingHelper.InOut(self, time)

		# ---------------------------------------- #
		# CUBIC EASING

		class Cubic (AnimationEasing):
			def In (time as single) as single:
				return (time * time * time)
			def Out (time as single) as single:
				return (Mathf.Pow(time-1,3) + 1)
			def InOut (time as single) as single:
				return EasingHelper.InOut(self, time)
	
		# ---------------------------------------- #
		# QUARTIC EASING

		class Quartic (AnimationEasing):
			def In (time as single) as single:
				return Mathf.Pow(time,4)
			def Out (time as single) as single:
				return (Mathf.Pow(time-1,4) - 1) * -1
			def InOut (time as single) as single:
				return EasingHelper.InOut(self, time)

		# ---------------------------------------- #
		# QUINTIC EASING

		class Quintic (AnimationEasing):
			def In (time as single) as single:
				return Mathf.Pow(time,5)
			def Out (time as single) as single:
				return (Mathf.Pow(time-1,5) + 1)
			def InOut (time as single) as single:
				return EasingHelper.InOut(self, time)
	
		# ---------------------------------------- #
		# SINUSOIDAL EASING

		class Sinusoidal (AnimationEasing):
			def In (time as single) as single:
				return Mathf.Sin((time-1)*(Mathf.PI/2)) + 1
			def Out (time as single) as single:
				return Mathf.Sin(time*(Mathf.PI/2))
			def InOut (time as single) as single:
				return EasingHelper.InOut(self, time)
	
		# ---------------------------------------- # 
		# EXPONENTIAL EASING

		class Exponential (AnimationEasing):
			def In (time as single) as single:
				return Mathf.Pow(2,10*(time-1))
			def Out (time as single) as single:
				return (-1 * Mathf.Pow(2,-10*time) + 1)
			def InOut (time as single) as single:
				return EasingHelper.InOut(self, time)

		# ---------------------------------------- #
		# CIRCULAR EASING

		class Circular (AnimationEasing):
			def In (time as single) as single:
				return (-1 * Mathf.Sqrt(1 - time*time) + 1)
			def Out (time as single) as single:
				return Mathf.Sqrt(1 - Mathf.Pow(time-1,2))
			def InOut (time as single) as single:
				return EasingHelper.InOut(self, time)

		# ---------------------------------------- #
		# BACK EASING

		class Back (AnimationEasing):
			s as single  = 1.70158
			s2 as single = 1.70158 * 1.525
			def In (time as single) as single:
				return time*time*((s+1)*time - s)
			def Out (time as single) as single:
				time = time - 1
				return (time*time*((s+1)*time + s) + 1)
			def InOut (time as single) as single:
				time = time*2
				if (time < 1):
					return 0.5*(time*time*((s2+1)*time - s2))
				else:
					time -= 2
					return 0.5*((time)*time*((s2+1)*time + s2) + 2)

		# ---------------------------------------- #
		# BOUNCE EASING

		class Bounce (AnimationEasing):
			def In (time as single) as single:
				return 1 - Out(1-time)
			def Out (time as single) as single:
				if (time < (1/2.75)):
					return (7.5625*time*time)
				elif (time < (2/2.75)):
					time -= (1.5/2.75)
					return (7.5625*time*time + .75)
				elif (time < (2.5/2.75)):
					time -= (2.25/2.75)
					return (7.5625*time*time + .9375)
				else:
					time -= (2.625/2.75)
					return (7.5625*time*time + .984375)
			def InOut (time as single) as single:
				return EasingHelper.InOut(self,time)

		# ---------------------------------------- #
		# ELASTIC EASING

		class Elastic (AnimationEasing):
			p as single = 0.3
			a as single = 1
	
			protected def Calc(time as single, dir as Easing.Direction) as single:
				s as single
		
				return time if time == 0 or time == 1
		
				if (a < 1):
					s = p/4
				else:
					s = p/(2*Mathf.PI) * Mathf.Asin(1/a)
		
				if (dir == Easing.In):
					time -= 1
					return -(a*Mathf.Pow(2,10*time)) * Mathf.Sin((time-s)*(2*Mathf.PI)/p)
				else:
					return a*Mathf.Pow(2,-10*time) * Mathf.Sin((time-s)*(2*Mathf.PI)/p) + 1
	
			def In (time as single) as single:
				return Calc(time,Easing.In)
	
			def Out (time as single) as single:
				return Calc(time,Easing.Out)
		
			def InOut (time as single) as single:
				return EasingHelper.InOut(self,time)
