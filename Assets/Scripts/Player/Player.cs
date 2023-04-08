using System;
using BrunoMikoski.AnimationSequencer;
using ScriptableObjects.Weapons;
using ScriptableObjects.Weapons.Guns;
using ScriptableObjects.Weapons.Melees;
using UnityEngine;
using UnityEngine.Serialization;
using Zenject;


enum AnimationState
{
    Idle = 1,
    Walk = 2,
    MeleeAttack = 3,
}


public class EquipmentInstance
{
    //Deep copy of equipment
    public Equipment Equipment;

    public Weapon WeaponOnHand;
    public Weapon LastWeaponOnHand;
    public EquipmentInstance(Equipment equipment)
    {
        Equipment = equipment;

        if (equipment.GunSocket1 != null) WeaponOnHand = equipment.GunSocket1;
        else if (equipment.GunSocket2 != null) WeaponOnHand = equipment.GunSocket2;
        else if (equipment.GunSocket3 != null) WeaponOnHand = equipment.GunSocket3;
        else if (equipment.MeleeSocket != null) WeaponOnHand = equipment.MeleeSocket;
        else Debug.LogError("All the weapons are null");

        LastWeaponOnHand = WeaponOnHand;
    }


    public bool AnyWeaponWithAmmoExists()
    {
        if (Equipment.GunSocket1 != null && Equipment.GunSocket1.ammo > 0) return true;
        if (Equipment.GunSocket2 != null && Equipment.GunSocket2.ammo > 0) return true;
        if (Equipment.GunSocket3 != null && Equipment.GunSocket3.ammo > 0) return true;
        return false;
    }
    
}

public class Player : MonoBehaviour
{
    private float _horizontalInput;
    private float _verticalInput;

    private FixedJoystick _fixedJoystick;

    private AnimationState _animationState;

    [SerializeField] private CharacterController characterController;
    [SerializeField] private Animator animator;
    [SerializeField] private AnimationSequencerController _animationSequencerController_Rifle;
    [SerializeField] private AnimationSequencerController _animationSequencerController_Pistol;
    [SerializeField] private AnimationSequencerController _animationSequencerController_Heavy;
    [SerializeField] private AnimationSequencerController _animationSequencerController_Melee;
    
    private float _defaultScale;
    private IItemManager _itemManager;
    private IPrefabCreator _prefabCreator;
    public EquipmentInstance EquipmentInstance;
    private IAudioManager _audioManager;
    private GameLevelUI _gameLevelUI;
    private CharacterWearer _characterWearer;

    
    [Inject]
    void Inject(IItemManager itemManager,IPrefabCreator prefabCreator,IAudioManager audioManager)
    {
        _audioManager = audioManager;
        _itemManager = itemManager;
        _prefabCreator = prefabCreator;
        EquipmentInstance = new EquipmentInstance(_itemManager.GetEquipment().GetCopy());
    }
    
    private void Start()
    {
        _defaultScale = transform.localScale.x;
        _gameLevelUI = FindObjectOfType<GameLevelUI>();
        _fixedJoystick = FindObjectOfType<FixedJoystick>();
        
        _characterWearer = GetComponentInChildren<CharacterWearer>();
        _characterWearer.HandleRightArmRotation(GetWeaponOnHand());
        _characterWearer.MapWeapon(GetWeaponOnHand());
        _characterWearer.MapCharacterWearings(EquipmentInstance.Equipment.Character);
        
    }

    void Update()
    {
        GetInputs();
        Move();
        HandleSpriteDirection();
        UpdateAnimationState();
    }
    

    

    
    
    

    

    //called by melee attack animation
    public void MeleeAttackOccured()
    {
        Weapon w = GetWeaponOnHand();

        if (w is Melee melee)
        {
            Vector3 position = _characterWearer.itemSocketMaps.WeaponSocket.transform.position;
            Vector3 localScale = transform.localScale;
            position.x += 2 * localScale.x;
            position.y += 0.1f;
            bool inverseDirection = localScale.x < 0;
            _prefabCreator.InstantiatePrefab(PrefabType.MuzzleFlash1,position,inverseDirection,0.1f,false);
            if(melee.hitSound)_audioManager.PlayAudioClip(melee.hitSound);
            _gameLevelUI.FireDone();
            characterController.Move(new Vector3(transform.localScale.x * 1, 0, 0));
        }
        
        
    }

    //called by melee attack animation
    public void MeleeAttackFinished()
    {
        _animationState = AnimationState.Idle;
    }
    
    public void MeleeButtonTapped()
    {
        Weapon w = GetWeaponOnHand();


        if (w is Melee)
        {
            if (_animationState != AnimationState.MeleeAttack)
            {
                _animationState = AnimationState.MeleeAttack;
                HandleFiringAnimation();
            }
        }
        else
        {
            HoldMelee();
        }
    }
    
    public void FireButtonTapped()
    {
        Weapon w = GetWeaponOnHand();

        if (w is Melee)
        {
            HoldLastWeapon();
        }
        else if (w is Gun gun)
        {
            if (gun.ammo > 0)
            {
                if (gun.gunType != GunType.Rifle)
                {
                    TriggerFire(gun);
                }
            }
            else
            {
                HoldNextGun();
            }

            
        }
    }
    
    
    
    public void FireButtonHold()
    {
        Weapon w = GetWeaponOnHand();
        if (w is Gun gun && gun.gunType == GunType.Rifle)
        {
            if(gun.ammo <= 0)return;
            TriggerFire(gun);
        }
    }
    
    
    
    //for guns
    private void TriggerFire(Gun gunOnHand)
    {
        if (gunOnHand.ammo <= 0)
        {
            if (EquipmentInstance.AnyWeaponWithAmmoExists())
            {
                HoldNextGun();
            }
            return;
        }
        
        if (gunOnHand.FireReady())
        {
            Vector3 position = _characterWearer.itemSocketMaps.WeaponSocket.transform.position;
            Vector3 localScale = transform.localScale;
            position.x += 2 * localScale.x;
            position.y += 0.1f;
            bool inverseDirection = localScale.x < 0;
            _prefabCreator.InstantiatePrefab(PrefabType.MuzzleFlash1,position,inverseDirection,0.1f,false);
            if(gunOnHand.fireSound)_audioManager.PlayAudioClip(gunOnHand.fireSound);
            _gameLevelUI.FireDone();
            HandleFiringAnimation();
        }
    }
    

    private void HandleFiringAnimation()
    {
        Weapon w = GetWeaponOnHand();
        
        _animationSequencerController_Rifle.Complete();
        _animationSequencerController_Pistol.Complete();
        _animationSequencerController_Heavy.Complete();
        _animationSequencerController_Melee.Complete();

        if (w is Gun gun)
        {
            if(gun.gunType == GunType.Rifle)
            {
                _animationSequencerController_Rifle.Play();
            }
            else if (gun.gunType == GunType.Pistol)
            {
                _animationSequencerController_Pistol.Play();
            }
            else if (gun.gunType == GunType.Heavy)
            {
                _animationSequencerController_Heavy.Play();
            }
        }
        
        else if (w is Melee)
        {
            _animationSequencerController_Melee.Play();
        }

    }


    private void HandleSpriteDirection()
    {
        //dont turn when attacking melee
        if(_animationState == AnimationState.MeleeAttack) return;
        
        Vector3 localScale = transform.localScale;
        if (_horizontalInput > 0.01f) localScale.x = _defaultScale;
        else if (_horizontalInput < -0.01f) localScale.x = -_defaultScale;
        transform.localScale = localScale;
    }


    private void GetInputs()
    {
        #if UNITY_EDITOR
            _horizontalInput = Input.GetAxisRaw("Horizontal");
            _verticalInput = Input.GetAxisRaw("Vertical");
        #else
            _horizontalInput = _fixedJoystick.Horizontal;
            _verticalInput = _fixedJoystick.Vertical;
        #endif
        
    }
    
    
    private void Move()
    {
        if(_animationState == AnimationState.MeleeAttack)return;



        Vector3 direction = new Vector3(_horizontalInput, _verticalInput, 0);
        direction.Normalize();
        characterController.Move(direction * (EquipmentInstance.Equipment.Character.movementSpeed * Time.deltaTime));
    }


    
    private void UpdateAnimationState()
    {
        if (_animationState == AnimationState.MeleeAttack)
        {
            
        }
        else if (Mathf.Abs(_horizontalInput) > 0.1f || Mathf.Abs(_verticalInput) > 0.1f)
        {
            _animationState = AnimationState.Walk;
        }
        else
        {
            _animationState = AnimationState.Idle;
        }

        
        animator.SetInteger("AnimationState",(int)_animationState);
    }
    
    
    public Weapon GetWeaponOnHand()
    {
        if (EquipmentInstance == null)
        {
            Debug.LogError("Equipment instance is null");
            return null;
        }

        return EquipmentInstance.WeaponOnHand;
    }

    public void HoldNextGun()
    {
        EquipmentInstance.LastWeaponOnHand = EquipmentInstance.LastWeaponOnHand;

        if (EquipmentInstance.WeaponOnHand == EquipmentInstance.Equipment.GunSocket1)
        {
            if (EquipmentInstance.Equipment.GunSocket2 != null)
            {
                EquipmentInstance.WeaponOnHand = EquipmentInstance.Equipment.GunSocket2;
            }
            else if (EquipmentInstance.Equipment.GunSocket3 != null)
            {
                EquipmentInstance.WeaponOnHand = EquipmentInstance.Equipment.GunSocket3;
            }
        }
        else if (EquipmentInstance.WeaponOnHand == EquipmentInstance.Equipment.GunSocket2)
        {
            if (EquipmentInstance.Equipment.GunSocket3 != null)
            {
                EquipmentInstance.WeaponOnHand = EquipmentInstance.Equipment.GunSocket3;
            }
            else if (EquipmentInstance.Equipment.GunSocket1 != null)
            {
                EquipmentInstance.WeaponOnHand = EquipmentInstance.Equipment.GunSocket1;
            }
        }
        else if (EquipmentInstance.WeaponOnHand == EquipmentInstance.Equipment.GunSocket3)
        {
            if (EquipmentInstance.Equipment.GunSocket1 != null)
            {
                EquipmentInstance.WeaponOnHand = EquipmentInstance.Equipment.GunSocket1;
            }
            else if (EquipmentInstance.Equipment.GunSocket2 != null)
            {
                EquipmentInstance.WeaponOnHand = EquipmentInstance.Equipment.GunSocket2;
            }
        }
        

        _gameLevelUI.WeaponChanged();
        _characterWearer.HandleRightArmRotation(GetWeaponOnHand());
        _characterWearer.MapWeapon(GetWeaponOnHand());
    }

    public void HoldLastWeapon()
    {
        EquipmentInstance.WeaponOnHand = EquipmentInstance.LastWeaponOnHand;
        
        _gameLevelUI.WeaponChanged();
        _characterWearer.HandleRightArmRotation(GetWeaponOnHand());
        _characterWearer.MapWeapon(GetWeaponOnHand());
    }
    
 
    public void HoldMelee()
    {
        EquipmentInstance.LastWeaponOnHand = EquipmentInstance.WeaponOnHand;
        EquipmentInstance.WeaponOnHand = EquipmentInstance.Equipment.MeleeSocket;

        _gameLevelUI.WeaponChanged();
        _characterWearer.MapWeapon(GetWeaponOnHand());
        _characterWearer.HandleRightArmRotation(GetWeaponOnHand());
    }

    
}
