namespace ReheeCmf
{
  public abstract class ProfileContainer : IWIthKeyType
  {
    public virtual Type KeyType => this.GetType().BaseType;
    protected Dictionary<string, Profile> Profiles { get; set; }

    protected ProfileContainer()
    {
      Profiles = new Dictionary<string, Profile>();
    }

    public virtual Profile? GetProfile(Enum key, string? keyOverride = null)
    {
      int intValue = Convert.ToInt32(key);
      return GetProfile(intValue == 0 ? keyOverride ?? "" : key.ToString());
    }
    public Profile? GetProfile(string key)
    {
      if (Profiles.TryGetValue(key, out var profile))
      {
        return profile;
      }
      return null;
    }
    public void AddProfile(Profile profile)
    {
      if (profile == null)
      {
        return;
      }

      var key = profile.EffectiveKey();
      if (string.IsNullOrEmpty(key))
      {
        return;
      }

      if (Profiles == null)
      {
        return;
      }

      Profiles.TryAdd(key, profile);
    }

    public bool RemoveProfile(string key, out Profile? value)
    {
      if (string.IsNullOrEmpty(key))
      {
        value = null;
        return false;
      }

      if (Profiles == null)
      {
        value = null;
        return false;
      }

      return Profiles.TryRemove(key, out value);
    }

    public IEnumerable<Profile> GetAllProfiles()
    {
      return Profiles.Values;
    }
  }
  public abstract class ProfileContainer<TKey, TProfile> : ProfileContainer
    where TKey : Enum
    where TProfile : Profile<TKey>
  {
    public override Type KeyType => typeof(TKey);
    public new TProfile? GetProfile(string key)
    {
      var profile = base.GetProfile(key);
      return profile as TProfile;
    }

    public TProfile? GetProfile(TKey enumKey, string keyOverride)
    {
      var profile = base.GetProfile(enumKey, keyOverride);
      return profile as TProfile;
    }

    public void AddProfile(TProfile profile)
    {
      base.AddProfile(profile);
    }

    public new IEnumerable<TProfile> GetAllProfiles()
    {
      return base.GetAllProfiles().OfType<TProfile>();
    }
  }
}
