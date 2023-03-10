namespace Furaffinity.Client.Models;

/// <summary>
/// Submission species wrapper.
/// </summary>
public class Species : IEqualityComparer<Species>
{
    private const string ExceptionTemplate = "Species: {0} not valid species";
    
    private static readonly Dictionary<string, string> SpeciesNameToIdMap = new()
    {
        {"unspecified / any", "1"},
        {"aardvark", "14001"},
        {"aardwolf", "14002"},
        {"aeromorph", "11001"},
        {"african wild dog", "14003"},
        {"airborne vehicle", "10001"},
        {"akita", "14004"},
        {"albatross", "14005"},
        {"alicorn (mlp)", "13001"},
        {"alien (other)", "5001"},
        {"alligator / crocodile", "7001"},
        {"alpaca", "14006"},
        {"amphibian (other)", "1000"},
        {"anaconda", "14007"},
        {"angel", "12001"},
        {"angel dragon", "11002"},
        {"anteater", "14008"},
        {"antelope", "6004"},
        {"aquatic (other)", "2000"},
        {"arachnid", "8000"},
        {"arctic fox", "14009"},
        {"argonian", "5002"},
        {"armadillo", "14010"},
        {"asari", "13002"},
        {"avali", "11012"},
        {"avian (other)", "3000"},
        {"axolotl", "14011"},
        {"baboon", "14012"},
        {"badger", "6045"},
        {"bangaa", "13003"},
        {"bat", "6001"},
        {"bear (other)", "6002"},
        {"beaver", "6064"},
        {"bee", "14013"},
        {"binturong", "14014"},
        {"bison", "14015"},
        {"blue jay", "14016"},
        {"border collie", "14017"},
        {"bovine (other)", "6007"},
        {"brown bear", "14018"},
        {"bubble dragon", "13004"},
        {"buffalo", "14019"},
        {"buffalo / bison", "14020"},
        {"bull terrier", "14021"},
        {"burmecian", "13005"},
        {"butterfly", "14022"},
        {"caiman", "14023"},
        {"camel", "6074"},
        {"canine (other)", "6017"},
        {"capybara", "14024"},
        {"caribou", "14025"},
        {"caterpillar", "14026"},
        {"centaur", "12002"},
        {"cephalopod", "2001"},
        {"cerberus", "12003"},
        {"cervine (other)", "6018"},
        {"chakat", "5003"},
        {"chameleon", "14027"},
        {"charr", "13006"},
        {"cheetah", "6021"},
        {"chicken", "14028"},
        {"chimera", "12004"},
        {"chimpanzee", "14029"},
        {"chinchilla", "14030"},
        {"chipmunk", "14031"},
        {"chiss", "13007"},
        {"chocobo", "5004"},
        {"chupacabra", "12005"},
        {"citra", "5005"},
        {"civet", "14032"},
        {"clouded leopard", "14033"},
        {"coatimundi", "14034"},
        {"cockatiel", "14035"},
        {"cockatrice", "12006"},
        {"corgi", "14036"},
        {"corvid", "3001"},
        {"cougar / puma", "6022"},
        {"cow", "6003"},
        {"coyote", "6008"},
        {"crab", "14037"},
        {"crane", "14038"},
        {"crayfish", "14039"},
        {"crow", "3002"},
        {"crustacean", "14040"},
        {"crux", "5006"},
        {"daemon", "5007"},
        {"dalmatian", "14041"},
        {"deathclaw", "13008"},
        {"deer", "14042"},
        {"demon", "12007"},
        {"dhole", "14043"},
        {"digimon", "5008"},
        {"dingo", "6011"},
        {"dinosaur", "8001"},
        {"displacer beast", "12008"},
        {"doberman", "6009"},
        {"dog (other)", "6010"},
        {"dolphin", "2002"},
        {"donkey / mule", "6019"},
        {"dracat", "5009"},
        {"draenei", "5010"},
        {"dragon (other)", "4000"},
        {"dragonborn", "12009"},
        {"drell", "13009"},
        {"drow", "12010"},
        {"duck", "3003"},
        {"dutch angel dragon", "11003"},
        {"dwarf", "12011"},
        {"eagle", "3004"},
        {"eastern dragon", "4001"},
        {"eel", "14044"},
        {"elcor", "13010"},
        {"elephant", "14045"},
        {"elf", "5011"},
        {"ewok", "13011"},
        {"exotic (other)", "5000"},
        {"falcon", "3005"},
        {"feline (other)", "6030"},
        {"felkin", "11011"},
        {"fennec", "6072"},
        {"ferret", "6046"},
        {"ferrin", "11004"},
        {"finch", "14046"},
        {"fish", "2005"},
        {"flamingo", "14047"},
        {"fossa", "14048"},
        {"fox (other)", "6075"},
        {"frog", "1001"},
        {"gargoyle", "5012"},
        {"gazelle", "6005"},
        {"gecko", "7003"},
        {"genet", "14049"},
        {"german shepherd", "6012"},
        {"gibbon", "14050"},
        {"giraffe", "6031"},
        {"goat", "6006"},
        {"goblin", "12012"},
        {"golem", "12013"},
        {"goose", "3006"},
        {"gorilla", "6054"},
        {"gray fox", "14051"},
        {"great dane", "14052"},
        {"grizzly bear", "14053"},
        {"gryphon", "3007"},
        {"guinea pig", "14054"},
        {"hamster", "14055"},
        {"hanar", "13012"},
        {"harpy", "12014"},
        {"hawk", "3008"},
        {"hedgehog", "6032"},
        {"hellhound", "12015"},
        {"heron", "14056"},
        {"hippogriff", "12016"},
        {"hippopotamus", "6033"},
        {"hobbit", "12017"},
        {"honeybee / bumblebee", "14057"},
        {"horse", "6034"},
        {"housecat", "6020"},
        {"hrothgar", "13013"},
        {"human", "6055"},
        {"humanoid", "14058"},
        {"hummingbird", "14059"},
        {"husky", "6014"},
        {"hybrid species", "10002"},
        {"hydra", "4002"},
        {"hyena", "6035"},
        {"iguana", "7004"},
        {"iksar", "5013"},
        {"imp", "12018"},
        {"impala", "14060"},
        {"incubus", "12019"},
        {"insect (other)", "8003"},
        {"jackal", "6013"},
        {"jackalope", "12020"},
        {"jaguar", "6023"},
        {"jogauni", "11005"},
        {"kaiju / giant monster", "5015"},
        {"kangaroo", "6038"},
        {"kangaroo mouse", "14061"},
        {"kangaroo rat", "14062"},
        {"kemonomimi", "13014"},
        {"khajiit", "13015"},
        {"kinkajou", "14063"},
        {"kirin", "12021"},
        {"kit fox", "14064"},
        {"kitsune", "12022"},
        {"koala", "6039"},
        {"kobold", "12023"},
        {"kodiak bear", "14065"},
        {"komodo dragon", "14066"},
        {"koopa", "13016"},
        {"krogan", "13017"},
        {"labrador", "14067"},
        {"lamia", "12024"},
        {"land vehicle", "10003"},
        {"langurhali", "5014"},
        {"lemur", "6056"},
        {"leopard", "6024"},
        {"liger", "14068"},
        {"linsang", "14069"},
        {"lion", "6025"},
        {"lizard", "7005"},
        {"llama", "6036"},
        {"lobster", "14070"},
        {"lombax", "13018"},
        {"longhair cat", "14071"},
        {"lynx", "6026"},
        {"magpie", "14072"},
        {"maine coon", "14073"},
        {"malamute", "14074"},
        {"mammal - feline", "14075"},
        {"mammal - herd", "14076"},
        {"mammal - marsupial", "14077"},
        {"mammal - mustelid", "14078"},
        {"mammal - other predator", "14079"},
        {"mammal - prey", "14080"},
        {"mammal - primate", "14081"},
        {"mammal - rodent", "14082"},
        {"mammal (other)", "6000"},
        {"manatee", "14083"},
        {"mandrill", "14084"},
        {"maned wolf", "14085"},
        {"manticore", "12025"},
        {"mantid", "8004"},
        {"marmoset", "14086"},
        {"marsupial (other)", "6042"},
        {"marten", "14087"},
        {"meerkat", "6043"},
        {"mimiga", "13019"},
        {"mink", "6048"},
        {"minotaur", "12026"},
        {"mobian", "13020"},
        {"mole", "14088"},
        {"mongoose", "6044"},
        {"monitor lizard", "14089"},
        {"monkey", "6057"},
        {"moogle", "5017"},
        {"moose", "14090"},
        {"moth", "14091"},
        {"mouse", "6065"},
        {"musk deer", "14092"},
        {"musk ox", "14093"},
        {"mustelid (other)", "6051"},
        {"naga", "5016"},
        {"neopet", "13021"},
        {"nephilim", "12027"},
        {"nevrean", "11006"},
        {"newt", "1002"},
        {"nu mou", "13022"},
        {"ocelot", "6027"},
        {"octopus", "14094"},
        {"okapi", "14095"},
        {"olingo", "14096"},
        {"opossum", "6037"},
        {"orangutan", "14097"},
        {"orc", "5018"},
        {"orca", "14098"},
        {"oryx", "14099"},
        {"ostrich", "14100"},
        {"otter", "6047"},
        {"owl", "3009"},
        {"panda", "6052"},
        {"pangolin", "14101"},
        {"panther", "6028"},
        {"parakeet", "14102"},
        {"parrot / macaw", "14103"},
        {"peacock", "14104"},
        {"pegasus", "12028"},
        {"penguin", "14105"},
        {"persian cat", "14106"},
        {"peryton", "12029"},
        {"phoenix", "3010"},
        {"pig / swine", "6053"},
        {"pigeon", "14107"},
        {"pika", "14108"},
        {"pine marten", "14109"},
        {"platypus", "14110"},
        {"pokemon", "5019"},
        {"polar bear", "14111"},
        {"pony", "6073"},
        {"pony (mlp)", "13023"},
        {"poodle", "14112"},
        {"porcupine", "14113"},
        {"porpoise", "2004"},
        {"primate (other)", "6058"},
        {"procyonid", "14114"},
        {"protogen", "11007"},
        {"protoss", "13024"},
        {"puffin", "14115"},
        {"quarian", "13025"},
        {"quoll", "6040"},
        {"rabbit / hare", "6059"},
        {"raccoon", "6060"},
        {"rat", "6061"},
        {"ray", "14116"},
        {"red fox", "14117"},
        {"red panda", "6062"},
        {"reindeer", "14118"},
        {"reptilian (other)", "7000"},
        {"reptillian", "14119"},
        {"rhinoceros", "6063"},
        {"robin", "14120"},
        {"robot / android / cyborg", "10004"},
        {"rodent (other)", "6067"},
        {"ronso", "13026"},
        {"rottweiler", "14121"},
        {"sabercats", "14122"},
        {"sabertooth", "14123"},
        {"salamander", "1003"},
        {"salarian", "13027"},
        {"sangheili / elites", "13028"},
        {"sasquatch", "12030"},
        {"satyr", "5020"},
        {"scorpion", "8005"},
        {"sea vehicle", "10005"},
        {"seagull", "14124"},
        {"seahorse", "14125"},
        {"seal", "6068"},
        {"secretary bird", "14126"},
        {"sergal", "5021"},
        {"serpent dragon", "4003"},
        {"serval", "14127"},
        {"shark", "2006"},
        {"sheep", "14128"},
        {"shiba inu", "14129"},
        {"shorthair cat", "14130"},
        {"shrew", "14131"},
        {"siamese", "14132"},
        {"sifaka", "14133"},
        {"silver fox", "14134"},
        {"skunk", "6069"},
        {"sloth", "14135"},
        {"snail", "14136"},
        {"snake / serpent", "7006"},
        {"snow leopard", "14137"},
        {"sparrow", "14138"},
        {"sphinx", "12031"},
        {"squid", "14139"},
        {"squirrel", "6070"},
        {"stoat", "14140"},
        {"stork", "14141"},
        {"succubus", "12032"},
        {"sugar glider", "14142"},
        {"sun bear", "14143"},
        {"swan", "3011"},
        {"swift fox", "14144"},
        {"synx", "11010"},
        {"tanuki", "5022"},
        {"tapir", "14145"},
        {"tasmanian devil", "14146"},
        {"tauntaun", "13029"},
        {"taur (other)", "5025"},
        {"tauren", "13030"},
        {"thylacine", "14147"},
        {"tiefling", "12033"},
        {"tiger", "6029"},
        {"toucan", "14148"},
        {"trandoshan", "13031"},
        {"transformer", "13032"},
        {"troll", "12034"},
        {"turian", "13033"},
        {"turtle / tortoise", "7007"},
        {"twi'lek", "13034"},
        {"unicorn", "5023"},
        {"viera", "13035"},
        {"vulpine (other)", "6015"},
        {"vulture", "14149"},
        {"wallaby", "6041"},
        {"walrus", "14150"},
        {"wasp", "14151"},
        {"water dragon", "12035"},
        {"weasel", "6049"},
        {"werewolf / lycanthrope", "12036"},
        {"western dragon", "4004"},
        {"whale", "2003"},
        {"wickerbeast", "11013"},
        {"wolf", "6016"},
        {"wolverine", "6050"},
        {"wookiee", "13036"},
        {"wyvern", "4005"},
        {"xenomorph", "5024"},
        {"yautja / predator", "13037"},
        {"yinglet", "11009"},
        {"yokai", "12037"},
        {"yordle", "13038"},
        {"yoshi", "13039"},
        {"zebra", "6071"},
        {"zerg", "13040"},
        {"zorgoia", "11008"},
    };

    internal Species(string speciesName)
    {
        if (string.IsNullOrWhiteSpace(speciesName))
        {
            throw new ArgumentNullException(nameof(speciesName));
        }

        if (!SpeciesNameToIdMap.TryGetValue(speciesName.ToLower(), out var id))
        {
            throw new InvalidOperationException(string.Format(ExceptionTemplate, speciesName));
        }

        SpeciesId = id;
    }
    
    /// <summary>
    /// Species id.
    /// </summary>
    public string SpeciesId { get; }

    /// <inheritdoc />
    public bool Equals(Species? x, Species? y)
    {
        if (ReferenceEquals(x, y)) return true;
        if (ReferenceEquals(x, null)) return false;
        if (ReferenceEquals(y, null)) return false;
        if (x.GetType() != y.GetType()) return false;
        return x.SpeciesId == y.SpeciesId;
    }

    /// <inheritdoc />
    public int GetHashCode(Species obj)
    {
        return obj.SpeciesId.GetHashCode();
    }
}