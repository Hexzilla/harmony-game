// SPDX-License-Identifier: MIT
pragma solidity >=0.4.21 <0.6.0;

import "./HRC721.sol";
import "@openzeppelin/contracts/token/ERC20/ERC20.sol";
import "@openzeppelin/contracts/math/SafeMath.sol";
import "@openzeppelin/contracts/utils/ReentrancyGuard.sol";
import "@openzeppelin/contracts/access/roles/MinterRole.sol";

contract Market is MinterRole, ReentrancyGuard {
  using SafeMath for uint64;
  using SafeMath for uint128;
  using SafeMath for uint256;

	// owner to benefit from primary sales
	address owner;

	// Stablecoin placeholder
	ERC20 public hrc20;

	// 721 tokens
	HRC721 hrc721;

	//sales
	mapping(address => uint256) public contributions;
	// uint256 public raised;

	mapping(address => uint256) public balances;

	//inventory
	struct Item {
		//tight pack 256bits
    uint64 minted;
		uint64 limit;
		uint128 price;
		//end tight pack
		address payable owner;
		string url;
  }

	Item[] private items;
	uint256 public totalItems;
	
  //constructor
	constructor(address _owner, ERC20 _hrc20, HRC721 _hrc721) public {		
		owner = _owner;
		hrc20 = _hrc20;
		hrc721 = _hrc721;
		balances[msg.sender] = 10000000;
	}


	function transfer(address _to, uint256 _value) public returns (bool success) {
		require(balances[msg.sender] >= _value);
		balances[msg.sender] -= _value;
		balances[_to] += _value;
		emit Transfer(msg.sender, _to, _value);
	}

	event Transfer(address indexed _from, address indexed _to, uint256 _value);

  /**
   * Only safe minting that passes purchase conditions
   */
	function _mint(address to, uint256 index) internal {
		uint64 minted = items[index].minted;
		uint64 limit = items[index].limit;
		require(minted < limit, "Crowdsale: item limit reached");
        items[index].minted++;
		hrc721.mintWithIndex(to, index);
	}

	/*
	 * Inventory management, Only the Minter
	 */
	function mint(address to, uint256 index) public onlyMinter {
		_mint(to, index);
	}

	//inventory management
	function addItem(uint64 limit, uint128 price, string memory url) public onlyMinter {
		items.push(Item(0, limit, price, msg.sender, url));
		totalItems++;
	}

	//inventory management
	function register(uint64 limit, uint128 price, string memory url) public {
		items.push(Item(0, limit, price, msg.sender, url));
		totalItems++;
	}

	//add item and mint
	function addItemAndMint(uint64 limit, uint128 price, string memory url, address to) public onlyMinter {
		addItem(limit, price, url);
		_mint(to, totalItems - 1);
	}

	/********************************
	Public Getters
	********************************/
	function getMinted(uint256 index) public view returns (uint64) {
		return items[index].minted;
	}

	function getLimit(uint256 index) public view returns (uint64) {
		return items[index].limit;
	}

	function getPrice(uint256 index) public view returns (uint128) {
		return items[index].price;
	}

	function getUrl(uint256 index) public view returns (string memory) {
		return items[index].url;
	}

	function getTokenData(uint256 tokenId) public view returns (string memory) {
		return items[hrc721.getItemIndex(tokenId)].url;
	}
}
