#include "HelloWorldScene.h"
#include "cocostudio/CocoStudio.h"
#include "ui/CocosGUI.h"
#include "ui/UIButton.h"

#include "HrExamples/HrHelloWorldView.h"
#include "HrExamples/HrHelloWorldMediator.h"

USING_NS_CC;
using namespace cocos2d::ui;

using namespace cocostudio::timeline;

Scene* HelloWorld::createScene()
{
    // 'scene' is an autorelease object
    auto scene = Scene::create();
    
    // 'layer' is an autorelease object
    auto layer = HelloWorld::create();

    // add layer as a child to scene
    scene->addChild(layer);

    // return the scene
    return scene;
}

// on "init" you need to initialize your instance
bool HelloWorld::init()
{
    //////////////////////////////
    // 1. super init first
    if ( !Layer::init() )
    {
        return false;
    }
    
    auto rootNode = CSLoader::createNode("MainScene.csb");
	m_pFrameRoot = std::make_shared<HrHelloWorldRoot>();
	m_pFrameRoot->Init();
	m_pFrameRoot->Start();

	auto pHelloworldLayer = new HrHelloWorldView();
	pHelloworldLayer->autorelease();
	pHelloworldLayer->HrInit();
	rootNode->addChild(pHelloworldLayer);
	m_pFrameRoot->BindViewToMediator<HrHelloWorldMediator>(pHelloworldLayer);

    addChild(rootNode);

    return true;
}
